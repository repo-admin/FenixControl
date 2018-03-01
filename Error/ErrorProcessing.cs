using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPC.Extensions.Convert;
using FenixHelper;
using FenixControl.Loggers;
using FenixControl.Sender;

namespace FenixControl.Error
{
	/// <summary>
	/// Zpracování chyby
	/// </summary>
	public class ErrorProcessing
	{
		public string ErrorMessage { get; set; }

		/// <summary>
		/// ctor
		/// </summary>
		public ErrorProcessing()
		{
			this.ErrorMessage = String.Empty;
		}

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="errorMessage"></param>
		public ErrorProcessing(string errorMessage)
		{
			this.ErrorMessage = errorMessage;
		}

		/// <summary>
		/// uložení chyby do logu
		/// <para>odeslání chyby emailem</para>
		/// <para>odeslání chyby SMS zprávou</para>		
		/// </summary>
		public void DoProcess()
		{
			Logger.WriteIntoLoggers(String.Empty, FileLogger.PrepareMsg(this.ErrorMessage), String.Empty);			
			EmailSender.SendMail(BC.EMAIL_SUBJECT_ERROR, this.ErrorMessage, false, BC.MailTo, "", "");
			SmsSender.SendSMS(BC.PhoneNumber, this.ErrorMessage);
		}
	}
}
