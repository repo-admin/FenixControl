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
using FenixControl.SmsSend;

namespace FenixControl.Sender
{
	public class SmsSender
	{
		public static void SendSMS(string phoneNumber, string smsMessage)
		{
			try
			{
				SmsSendSoapClient client = new SmsSendSoapClient("SmsSendSoap");
				client.SendMessage(phoneNumber, String.Empty, smsMessage, BC.SMS_SENDER, BC.SMS_SENDER_ID, String.Empty);
				client.Close();
			}
			catch (Exception ex)
			{				
				ProcResult result = BC.CreateErrorResult(FenixHelper.AppLog.GetMethodName(), ex);
				Logger.WriteIntoLoggers(String.Empty, result.ReturnMessage, String.Empty);				
			}			
		}
	}
}
