using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FenixControl.Sender
{
	public class EmailSender
	{
		/// <summary>Odeslání mailu.</summary>
		/// <param name="mailSubject">Předmět mailu.</param>
		/// <param name="mailBody">Tělo mailu.</param>
		/// <param name="isBodyHtml">Příznak, jseslti je tělo formátu html.</param>
		/// <param name="mailTo">Adresy příjemců oddělené středníkem. Povinná je alespoň jeda adresa.</param>
		/// <param name="mailCC">Adresy příjemců kopie oddělené středníkem.</param>
		/// <param name="mailBcc">Adresy příjemců slepé kopie oddělené středníkem.</param>
		public static bool SendMail(string mailSubject, string mailBody, bool isBodyHtml, string mailTo, string mailCC, string mailBcc)
		{
			return EmailSender.SendMail(mailSubject, mailBody, isBodyHtml, mailTo, mailCC, mailBcc, null);
		}

		/// <summary>Odeslání mailu.</summary>
		/// <param name="mailSubject">Předmět mailu.</param>
		/// <param name="mailBody">Tělo mailu.</param>
		/// <param name="isBodyHtml">Příznak, jseslti je tělo formátu html.</param>
		/// <param name="mailTo">Adresy příjemců oddělené středníkem. Povinná je alespoň jeda adresa.</param>
		/// <param name="mailCC">Adresy příjemců kopie oddělené středníkem.</param>
		/// <param name="mailBcc">Adresy příjemců slepé kopie oddělené středníkem.</param>
		/// <param name="attachments">Kolekce příloh.</param>
		public static bool SendMail(string mailSubject, string mailBody, bool isBodyHtml, string mailTo, string mailCC, string mailBcc, List<Attachment> attachments)
		{
			bool retVal = false;
			try
			{
				char[] delims = { ';', ',' };

				// Adresy příjemců
				MailAddressCollection addrsTo = new MailAddressCollection();
				if (String.IsNullOrWhiteSpace(mailTo) == false)
				{
					string[] addrs = mailTo.Split(delims);
					for (int i = 0; i < addrs.Length; i++)
					{
						addrsTo.Add(new MailAddress(addrs[i]));
					}
				}

				// Adresy příjemců kopie
				MailAddressCollection addrsCC = new MailAddressCollection();
				if (String.IsNullOrWhiteSpace(mailCC) == false)
				{
					string[] addrs = mailCC.Split(delims);
					for (int i = 0; i < addrs.Length; i++)
					{
						addrsCC.Add(new MailAddress(addrs[i]));
					}
				}

				// Adresy příjemců slepé kopie
				MailAddressCollection addrsBcc = new MailAddressCollection();
				if (String.IsNullOrWhiteSpace(mailBcc) == false)
				{
					string[] addrs = mailBcc.Split(delims);
					for (int i = 0; i < addrs.Length; i++)
					{
						addrsBcc.Add(new MailAddress(addrs[i]));
					}
				}

				retVal = EmailSender.SendMail(mailSubject, mailBody, isBodyHtml, addrsTo, addrsCC, addrsBcc, attachments);
			}
			catch { }

			return retVal;
		}

		/// <summary>Odeslání mailu.</summary>
		/// <param name="mailSubject">Předmět mailu.</param>
		/// <param name="mailBody">Tělo mailu.</param>
		/// <param name="isBodyHtml">Příznak, jseslti je tělo formátu html.</param>
		/// <param name="mailsTo">Adresy příjemců. Povinná je alespoň jeda adresa.</param>
		/// <param name="mailsCC">Adresy příjemců kopie.</param>
		/// <param name="mailsBcc">Adresy příjemců slepé kopie.</param>
		/// <param name="attachments">Kolekce příloh.</param>
		public static bool SendMail(string mailSubject, string mailBody, bool isBodyHtml, MailAddressCollection mailsTo, MailAddressCollection mailsCC, MailAddressCollection mailsBcc, List<Attachment> attachments)
		{
			if (mailsTo == null || mailsTo.Count <= 0) return false;
			char[] delims = { ';', ',' };

			bool retVal = false;
			try
			{

				string mailServer = BC.MailServer;
				if (String.IsNullOrWhiteSpace(mailServer) == false)
				{
					using (MailMessage mailMsg = new MailMessage(new MailAddress(BC.MailFrom), mailsTo[0]))
					{
						for (int i = 1; i < mailsTo.Count; i++)
						{
							mailMsg.To.Add(mailsTo[i]);
						}

						// Kopie mailu
						if (mailsCC != null)
						{
							for (int i = 0; i < mailsCC.Count; i++)
							{
								if (mailMsg.CC.IndexOf(mailsCC[i]) < 0)
									mailMsg.CC.Add(mailsCC[i]);
							}
						}

						// Slepé kopie mailu
						if (mailsBcc != null)
						{
							for (int i = 0; i < mailsBcc.Count; i++)
							{
								if (mailMsg.Bcc.IndexOf(mailsBcc[i]) < 0)
									mailMsg.Bcc.Add(mailsBcc[i]);
							}
						}

						mailMsg.IsBodyHtml = isBodyHtml;
						mailMsg.Subject = mailSubject;
						mailMsg.Body = mailBody;	//Replace(Environment.NewLine, "<br />")
						mailMsg.Priority = MailPriority.Normal;
						if (attachments != null && attachments.Count > 0)
						{
							foreach (var attm in attachments)
							{
								mailMsg.Attachments.Add(attm);
							}
						}

						SmtpClient smtp = new SmtpClient(mailServer);
						smtp.Send(mailMsg);
						retVal = true;
					}
				}
			}
			catch { }

			return retVal;
		}
	}
}
