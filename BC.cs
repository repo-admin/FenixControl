using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using FenixControl.Loggers;
using FenixControl.Sender;

namespace FenixControl
{
	internal class BC
	{
		internal const string APP_NAMESPACE = "FenixControl";

		internal const string EMAIL_SUBJECT = "Fenix Control";

		internal const string EMAIL_SUBJECT_ERROR = "Fenix Control Error";

		internal const string SMS_SENDER = "Rezler";

		internal const string SMS_SENDER_ID = "1084";

		/// <summary>
		/// OK
		/// <value>0</value>
		/// </summary>
		internal const int OK = 0;

		/// <summary>
		/// Not OK
		/// <value>-1</value>
		/// </summary>
		internal const int NOT_OK = -1;

		internal const string UNKNOWN = "Unknown";

		private const string LOG_FILE = "FenixControl.log";

		#region Properties

		internal static string LogFile
		{			
			get 
			{
				string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				return Path.Combine(assemblyPath, LOG_FILE); 
			}
		}

		internal static int ApplicationMode
		{
			get { try { return int.Parse(ConfigurationManager.AppSettings["ApplicationMode"].Trim()); } catch { return 1; } }
		}

		internal static string MailServer
		{
			get { try { return ConfigurationManager.AppSettings["MailServer"].Trim(); } catch { return "relay.upc.cz"; } }
		}

		internal static string MailFrom
		{
			get { try { return ConfigurationManager.AppSettings["MailFrom"].Trim(); } catch { return "control.fenix@upc.cz"; } }
		}

		internal static string MailTo
		{
			get { try { return ConfigurationManager.AppSettings["MailTo"].Trim(); } catch { return "michal.rezler@upc.cz"; } }
		}

		internal static string MailSelfTest
		{
			get { try { return ConfigurationManager.AppSettings["MailSelfTest"].Trim(); } catch { return "michal.rezler@upc.cz"; } }
		}
				
		internal static string PhoneNumber
		{
			get { try { return ConfigurationManager.AppSettings["PhoneNumber"].Trim(); } catch { return "778489631"; } }
		}

		internal static string Login
		{
			get { try { return ConfigurationManager.AppSettings["Login"].Trim(); } catch { return "admin"; } }
		}

		internal static string Password
		{
			get { try { return ConfigurationManager.AppSettings["Password"].Trim(); } catch { return "Heslo*123"; } }
		}
		                                                                                              
		#endregion

		/// <summary>
		/// vytvoří chybový ProcResult
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="ex"></param>
		/// <returns></returns>
		internal static ProcResult CreateErrorResult(string methodName, Exception ex)
		{
			ProcResult result = new ProcResult();

			result.ReturnValue = BC.NOT_OK;
			result.ReturnMessage = String.Format("{0}{1}{2}", methodName, Environment.NewLine, ex.Message);

			if (ex.InnerException != null)
				result.ReturnMessage = result.ReturnMessage + Environment.NewLine + ex.InnerException.Message;

			return result;
		}

		/// <summary>
		/// vytvoří chybové hlášení
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
		/// 
		/// </summary>
		internal static void SelfTest()
		{
			try
			{
				string message = "Self test";
				EmailSender.SendMail(BC.EMAIL_SUBJECT, message, false, BC.MailSelfTest, "", "");
				SmsSender.SendSMS(BC.PhoneNumber, message);
			}
			catch (Exception ex)
			{
				ProcResult result = BC.CreateErrorResult(FenixHelper.AppLog.GetMethodName(), ex);
				Logger.WriteIntoLoggers(String.Empty, result.ReturnMessage, String.Empty);				
			}
		}
	}
}
