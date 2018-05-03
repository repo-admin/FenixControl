using System;

namespace Fenix
{
	/// <summary>
	/// Obecná třída logger
	/// </summary>
	public class Logger
	{
        /// <summary>
        /// Zápis zprávy do logů specifikované typem zprávy, informací zda přidat časové razítko do zprávy, zprávou a názvem metody
        /// </summary>
        /// <param name="messageType">Typ zprávy</param>
        /// <param name="addDateTime">Hodnota, zda-li přidat do logovací zprávy časové razítko, či nikoliv</param>
        /// <param name="message">Textová hodnota logovací zprávy</param>
        /// <param name="methodName">Název metody</param>
        public static void WriteIntoLoggers(string messageType, bool addDateTime, string message, string methodName)
		{
			if (addDateTime)
			{
				FileLogger.WriteToLog(Bc.LogFile, FileLogger.PrepareMsg(message));
			}
			else
			{
				FileLogger.WriteToLog(Bc.LogFile, message);
			}
		}

        /// <summary>
        /// Zápis zprávy do logů specifikované typem zprávy, zprávou a názvem metody
        /// </summary>
        /// <param name="messageType">Typ zprávy</param>
        /// <param name="message">Textová hodnota logovací zprávy</param>
        /// <param name="methodName">Název metody</param>
        public static void WriteIntoLoggers(string messageType, string message, string methodName)
		{
			FileLogger.WriteToLog(Bc.LogFile, message);
		}

	    /// <summary>
	    /// Zápis zprávy do logů specifikované typem zprávy, vyjímkou a názvem metody
	    /// </summary>
	    /// <param name="messageType">Typ zprávy</param>
	    /// <param name="exception">Instance chyby, která se má zapsat do logu</param>
	    /// <param name="methodName">Název metody</param>
	    public static void WriteIntoLoggers(string messageType, Exception exception, string methodName)
		{
			FileLogger.WriteToLog(Bc.LogFile, FileLogger.PrepareMsg(exception.Message));
		}
	}
}
