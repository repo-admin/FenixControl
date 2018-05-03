using System;
using Fenix.SmsSend;

namespace Fenix
{
	public class SmsSender
	{
		public static void SendSMS(string phoneNumber, string smsMessage)
		{
			try
			{
				SmsSendSoapClient client = new SmsSendSoapClient("SmsSendSoap");
				client.SendMessage(phoneNumber, String.Empty, smsMessage, Bc.SmsSender, Bc.SmsSenderId, String.Empty);
				client.Close();
			}
			catch (Exception ex)
			{				
				ProcResult result = Bc.CreateErrorResult(Fenix.AppLog.GetMethodName(), ex);
				Logger.WriteIntoLoggers(String.Empty, result.ReturnMessage, String.Empty);				
			}			
		}
	}
}
