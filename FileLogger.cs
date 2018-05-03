using System;
using System.Text;

namespace Fenix
{
	/// <summary>
	/// Logování (do souboru na disku)
	/// </summary>
	public class FileLogger
	{
        #region Public Methods

        /// <summary>Zápis do log souboru.</summary>
        /// <param name="logFile">Cesta k log souboru</param>
        /// <param name="msg">Textová hodnota logovací zprávy</param>
        public static void WriteToLog(string logFile, string msg)
		{
			System.IO.StreamWriter wr = null;
			try
			{
				wr = new System.IO.StreamWriter(logFile, true, Encoding.GetEncoding(1250));
				wr.WriteLine(msg);
				wr.Flush();
				wr.Close();
			}
			catch
			{
				if (wr != null)
					wr.Close();
			}
		}

		/// <summary>
		/// Příprava zprávy (je rozšířena o datum a čas)
		/// </summary>
		/// <param name="message">Textová hodnota logovací zprávy</param>
		/// <returns></returns>
		public static string PrepareMsg(string message)
		{ 
			return String.IsNullOrEmpty(message) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") 
				                                 : String.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);			
		}

		/// <summary>
		/// Smazání log souboru, pokud přesáhne definovanou velikost
		/// </summary>
		/// <param name="logFile">Cesta k souboru</param>
		public static void DeleteLogFile(string logFile)
		{
			// odstranění LOGu, je-li větší než 100 MB
			try
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(logFile);
				if (fi.Length > (100 * 1024 * 1024))
					fi.Delete();
			}
			catch { }
		}
	
		#endregion
	}
}
