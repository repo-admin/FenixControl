using System;

namespace Fenix
{
    class Program
	{
		

		static void Main(string[] args)
		{
			switch (Bc.ApplicationMode)
			{
				case (int)ApplicationMode.Run:
					Logger.WriteIntoLoggers(String.Empty, FileLogger.PrepareMsg("Fenix Control spuštěn..."), String.Empty);
					FenixStatusReader fenixStatusReader = new FenixStatusReader();
					fenixStatusReader.ReadAndEvaluateStatus();
					Logger.WriteIntoLoggers(String.Empty, FileLogger.PrepareMsg("Fenix Control ukončen..."), String.Empty);
					break;

				case (int)ApplicationMode.ExecutiveProhibited:
					Logger.WriteIntoLoggers(String.Empty, FileLogger.PrepareMsg("Fenix Control má konfigurací zakázanou prováděcí část..."), String.Empty);
					break;

				case (int)ApplicationMode.SelfTest:					
					Logger.WriteIntoLoggers(String.Empty, FileLogger.PrepareMsg("Fenix Control self test"), String.Empty);
					Bc.SelfTest();
					break;
					
				default:
					ErrorProcessing errorProcessing = new ErrorProcessing("Neznámá hodnota cfg klíče [ApplicationMode]");					
					errorProcessing.DoProcess();					
					break;
			}

			Logger.WriteIntoLoggers(String.Empty, String.Empty, String.Empty);
		}
	}
}
