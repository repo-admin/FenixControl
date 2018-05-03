using System;

namespace Fenix
{
	/// <summary>
	/// Třída sloužící ke zpracování chyby
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
		/// Uložení chyby do logu
		/// <para>odeslání chyby emailem</para>
		/// <para>odeslání chyby SMS zprávou</para>		
		/// </summary>
		public void DoProcess()
		{
			Logger.WriteIntoLoggers(String.Empty, FileLogger.PrepareMsg(this.ErrorMessage), String.Empty);			
			EmailSender.SendMail(Bc.EmailSubjectError, this.ErrorMessage, false, Bc.MailTo, "", "");
			SmsSender.SendSMS(Bc.PhoneNumber, this.ErrorMessage);
		}
	}
}
