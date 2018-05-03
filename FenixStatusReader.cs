using System;
using System.Text;
using Fenix.FenixSoapService;

namespace Fenix
{
	/// <summary>
	/// Třída pro čtení statusu (ověřuje se: FenixSoapService, FenixAppService, FenixAutomat a databáze)
	/// </summary>
	public class FenixStatusReader
	{
        /// <summary>
        /// Vrací instanci <seealso cref="ErrorProcessing"/>
        /// </summary>
		private static ErrorProcessing ErrProcessing { get; set; }

		/// <summary>
		/// ctor
		/// </summary>
		static FenixStatusReader()
		{
			ErrProcessing = new ErrorProcessing();
		}

		/// <summary>
		/// Přečte a vyhodnotí status
		/// </summary>
		public void ReadAndEvaluateStatus()
		{
			try
			{
				SubmitDataToProcessingResult processingResult = null;

				byte[] byteU8 = Encoding.UTF8.GetBytes("Jen proto, aby se do volani metody na serveru nepredavala null hotnota.");

				FenixSoapWebSvcSoapClient client = new FenixSoapWebSvcSoapClient("FenixSoapWebSvcSoap");
				processingResult = client.SubmitDataToProcessing(Bc.Login, Bc.Password, Environment.MachineName, "GetServicesStatuses", byteU8, "UTF-8");
				
				if (processingResult.MessageNumber != Bc.Ok)
				{
					ErrProcessing.ErrorMessage = processingResult.Errors[0].ErrorMessage;
					ErrProcessing.DoProcess();
				}
				else
				{
					ProcResult result = EvaluateStatus(processingResult.MessageDescription);
					if (result.ReturnValue != Bc.Ok)
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
        /// Vyhodnocení statusu
        /// </summary>
        /// <param name="activeSqlContent">Textová hodnota ve formátu 'Automat Rows : 1  |   DateTime : yyyy-mm-dd hh:mi:ss.mmm'</param>
        /// <returns></returns>
        private ProcResult EvaluateStatus(string activeSqlContent)
		{
			ProcResult result = new ProcResult();
			result.ReturnValue = Bc.NotOk;

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

				result.ReturnValue = Bc.Ok;
			}
			catch (Exception ex)
			{
				result = Bc.CreateErrorResult(AppLog.GetMethodName(), ex);
			}
			
			return result;
		}
	}
}
