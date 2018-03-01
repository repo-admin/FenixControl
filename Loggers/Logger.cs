using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenixControl.Sender;
using FenixHelper;

namespace FenixControl.Loggers
{
	/// <summary>
	/// Obecná třída logger
	/// </summary>
	public class Logger
	{		
		/// <summary>
		/// zápis zprávy do logů
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		/// <param name="methodName"></param>
		/// <param name="zicyzID"></param>
		public static void WriteIntoLoggers(string messageType, bool addDateTime, string message, string methodName)
		{
			if (addDateTime)
			{
				FileLogger.WriteToLog(BC.LogFile, FileLogger.PrepareMsg(message));
			}
			else
			{
				FileLogger.WriteToLog(BC.LogFile, message);
			}
		}

		/// <summary>
		/// zápis zprávy do logů
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		/// <param name="methodName"></param>
		/// <param name="zicyzID"></param>
		public static void WriteIntoLoggers(string messageType, string message, string methodName)
		{
			FileLogger.WriteToLog(BC.LogFile, message);
		}

		/// <summary>
		/// zápis zprávy do logů
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="exception"></param>
		/// <param name="methodName"></param>
		/// <param name="zicyzID"></param>
		public static void WriteIntoLoggersORG(string messageType, Exception exception, string methodName)
		{
			FileLogger.WriteToLog(BC.LogFile, FileLogger.PrepareMsg(exception.Message));
		}
	}
}
