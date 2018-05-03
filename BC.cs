using System;
using System.Configuration;
using System.IO;
using Fenix.FenixSoapService;

namespace Fenix
{
    /// <summary>
    /// Třída obsahující konstanty anebo hodnoty z app config souboru
    /// </summary>
	internal class Bc
	{
        /// <summary>
        /// Jmenný prostor aplikace
        /// </summary>
		internal const string AppNamespace = "Fenix";

        /// <summary>
        /// Předmět (defaultní) emailové zprávy
        /// </summary>
		internal const string EmailSubject = "Fenix Control";

        /// <summary>
        /// Předmět (chybové) emailové zprávy
        /// </summary>
		internal const string EmailSubjectError = "Fenix Control Error";

        /// <summary>
        /// Název odesílatele
        /// </summary>
		internal const string SmsSender = "Rezler";

        /// <summary>
        /// Id odesílatele
        /// </summary>
		internal const string SmsSenderId = "1084";

		/// <summary>
		/// Hodnota odpovědi - 'OK'
		/// <value>0</value>
		/// </summary>
		internal const int Ok = 0;

        /// <summary>
        /// Hodnota odpovědi - 'NOT OK'
        /// <value>-1</value>
        /// </summary>
        internal const int NotOk = -1;

	    /// <summary>
        /// Název log souboru
        /// </summary>
		private const string LogFileName = "FenixControl.log";

		#region Properties

        /// <summary>
        /// Systémová (úplná) cesta k log souboru
        /// </summary>
		internal static string LogFile
		{			
			get 
			{
				string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				return Path.Combine(assemblyPath, LogFileName); 
			}
		}

        /// <summary>
        /// Vrací číselnou hodnotu <seealso cref="ApplicationMode"/> specifikovaného v konfiguračním souboru
        /// </summary>
		internal static int ApplicationMode
		{
			get { try { return int.Parse(ConfigurationManager.AppSettings["ApplicationMode"].Trim()); } catch { return 1; } }
		}

        /// <summary>
        /// Vrací název mail serveru specifikovaného v konfiguračním souboru, anebo defaultní hodnotu 'relay.upc.cz'
        /// </summary>
        internal static string MailServer
		{
			get { try { return ConfigurationManager.AppSettings["MailServer"].Trim(); } catch { return "relay.upc.cz"; } }
		}

        /// <summary>
        /// Vrací jméno odesílatele specifikovaného v konfiguračním souboru, anebo defaultní hodnotu 'control.fenix@upc.cz'
        /// </summary>
        internal static string MailFrom
		{
			get { try { return ConfigurationManager.AppSettings["MailFrom"].Trim(); } catch { return "control.fenix@upc.cz"; } }
		}

        /// <summary>
        /// Vrací jméno příjemce specifikovaného v konfiguračním souboru, anebo defaultní hodnotu 'michal.rezler@upc.cz'
        /// </summary>
		internal static string MailTo
		{
			get { try { return ConfigurationManager.AppSettings["MailTo"].Trim(); } catch { return "michal.rezler@upc.cz"; } }
		}

        /// <summary>
        /// Vrací jméno příjemce (pro test) specifikovaného v konfiguračním souboru, anebo defaultní hodnotu 'michal.rezler@upc.cz'
        /// </summary>
		internal static string MailSelfTest
		{
			get { try { return ConfigurationManager.AppSettings["MailSelfTest"].Trim(); } catch { return "michal.rezler@upc.cz"; } }
		}

        /// <summary>
        /// Vrací mobilní číslo pro příjem sms specifikovaného v konfiguračním souboru, anebo defaultní hodnotu '778489631'
        /// </summary>
        internal static string PhoneNumber
		{
			get { try { return ConfigurationManager.AppSettings["PhoneNumber"].Trim(); } catch { return "778489631"; } }
		}

        /// <summary>
        /// Vrací přihlašovací jméno pro volání metody <seealso cref="FenixSoapWebSvcSoapClient.SubmitDataToProcessing"/> specifikovaného
        /// v konfiguračním souboru, anebo defaultní hodnotu 'admin'
        /// </summary>
		internal static string Login
		{
			get { try { return ConfigurationManager.AppSettings["Login"].Trim(); } catch { return "admin"; } }
		}

        /// <summary>
        /// Vrací heslo pro volání metody <seealso cref="FenixSoapWebSvcSoapClient.SubmitDataToProcessing"/> specifikovaného
        /// v konfiguračním souboru, anebo defaultní hodnotu 'Heslo*123'
        /// </summary>
		internal static string Password
		{
			get { try { return ConfigurationManager.AppSettings["Password"].Trim(); } catch { return "Heslo*123"; } }
		}

        #endregion

        /// <summary>
        /// Vytvoří chybový <seealso cref="ProcResult"/>
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        internal static ProcResult CreateErrorResult(string methodName, Exception ex)
		{
			ProcResult result = new ProcResult();

			result.ReturnValue = Bc.NotOk;
			result.ReturnMessage = String.Format("{0}{1}{2}", methodName, Environment.NewLine, ex.Message);

			if (ex.InnerException != null)
				result.ReturnMessage = result.ReturnMessage + Environment.NewLine + ex.InnerException.Message;

			return result;
		}

		/// <summary>
		/// Vytvoří chybové hlášení
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		internal static string CreateErrorMessage(string methodName, Exception exception)
		{
			string errorMessage = String.Format("{0}{1}{2}", methodName, Environment.NewLine, exception.Message);
			if (exception.InnerException != null)
				errorMessage += Environment.NewLine + exception.InnerException.Message;

			return errorMessage;
		}

		/// <summary>
		/// Provede self test odeslání emailu
		/// </summary>
		internal static void SelfTest()
		{
			try
			{
				string message = "Self test";
				EmailSender.SendMail(Bc.EmailSubject, message, false, Bc.MailSelfTest, "", "");
				Fenix.SmsSender.SendSMS(Bc.PhoneNumber, message);
			}
			catch (Exception ex)
			{
				ProcResult result = Bc.CreateErrorResult(AppLog.GetMethodName(), ex);
				Logger.WriteIntoLoggers(String.Empty, result.ReturnMessage, String.Empty);				
			}
		}
	}
}
