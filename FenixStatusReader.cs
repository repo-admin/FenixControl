using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPC.Extensions.Convert;
using FenixHelper;
using FenixControl.Error;
using FenixControl.FenixSoapService;

namespace FenixControl
{
	/// <summary>
	/// Třída pro čtení statusu 
	/// (ověřuje se: FenixSoapService, FenixAppService, FenixAutomat a databáze)
	/// </summary>
	public class FenixStatusReader
	{
		private static ErrorProcessing ErrProcessing { get; set; }

		/// <summary>
		/// ctor
		/// </summary>
		static FenixStatusReader()
		{
			ErrProcessing = new ErrorProcessing();
		}

		/// <summary>
		/// přečte a vyhodnotí status
		/// </summary>
		public void ReadAndEvaluateStatus()
		{
			try
			{
				SubmitDataToProcessingResult processingResult = null;

				byte[] byteU8 = Encoding.UTF8.GetBytes("Jen proto, aby se do volani metody na serveru nepredavala null hotnota.");

				FenixSoapWebSvcSoapClient client = new FenixSoapWebSvcSoapClient("FenixSoapWebSvcSoap");
				processingResult = client.SubmitDataToProcessing(BC.Login, BC.Password, Environment.MachineName, "GetServicesStatuses", byteU8, "UTF-8");
				
				if (processingResult.MessageNumber != BC.OK)
				{
					ErrProcessing.ErrorMessage = processingResult.Errors[0].ErrorMessage;
					ErrProcessing.DoProcess();
				}
				else
				{
					ProcResult result = evaluateStatus(processingResult.MessageDescription);
					if (result.ReturnValue != BC.OK)
					{
						ErrProcessing.ErrorMessage = result.ReturnMessage;
						ErrProcessing.DoProcess();
					}
				}
			}
			catch (Exception ex)
			{
				ErrProcessing.ErrorMessage = ex.Message;
				ErrProcessing.DoProcess();
			}
		}

		/// <summary>
		/// očekává se string 'Automat Rows : 1  |   DateTime : yyyy-mm-dd hh:mi:ss.mmm'
		/// </summary>
		/// <param name="activeSqlContent"></param>
		/// <returns></returns>
		private ProcResult evaluateStatus(string activeSqlContent)
		{
			ProcResult result = new ProcResult();
			result.ReturnValue = BC.NOT_OK;

			try
			{				
				string numberInSqlContent = String.Empty;
				string[] sqlSplit = activeSqlContent.Split('|');
				foreach (char item in sqlSplit[0])
				{
					if (char.IsDigit(item))
					{
						numberInSqlContent += item.ToString();
					}
				}

				long numRow = long.Parse(numberInSqlContent);

				if (numRow == 0)
				{
					throw new Exception(String.Format("Nalezený počet záznamů je 0. Očekáván 1 záznam."));
				}

				result.ReturnValue = BC.OK;
			}
			catch (Exception ex)
			{
				result = BC.CreateErrorResult(FenixHelper.AppLog.GetMethodName(), ex);
			}
			
			return result;
		}
	}
}
