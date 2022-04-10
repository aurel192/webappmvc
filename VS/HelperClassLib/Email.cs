using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace HelperClassLib
{
    public static class Email
    {
        public static void SendPost(string recipient, string subject, string msg, bool showOnConsole = false)
        {
            try
            {
                if (showOnConsole)
                {
                    string showMsg = DateTime.Now.ToDateTimeString() + "\nMAIL SUB = " + subject + "\nMAIL MSG = " + msg;
                    Console.WriteLine("\n\n" + showMsg + "\n\n");
                }
                List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
                kvpList.Add(new KeyValuePair<string, string>("psw", Constants.psw));
                kvpList.Add(new KeyValuePair<string, string>("recipient", recipient));
                kvpList.Add(new KeyValuePair<string, string>("subject", subject));
                kvpList.Add(new KeyValuePair<string, string>("msg", msg));
                var result = Http.Post("https://www.collectioninventory.com", "mailsender/send", kvpList).Result;
            }
            catch (Exception ex)
            {

            }
        }

        // TODO: MEGÁLL A SEND(mail)-nél
        public static void Send(string recipient, string subject, string msg)
        {
            try
            {
                MailMessage mail = new MailMessage("error@collectioninventory.com", recipient);
                SmtpClient client = new SmtpClient();
                client.Port = 465;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("error@collectioninventory.com", "Smd192192");
                client.Host = "mail.tiscan.hu";
                mail.Subject = subject;
                mail.Body = msg;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Send2(string recipient, string subject, string msg)
        {
            try
            {
                MailMessage message = new System.Net.Mail.MailMessage();
                string fromEmail = "error@collectioninventory.com";
                string password = "Smd192192";
                string toEmail = recipient;
                message.From = new MailAddress(fromEmail);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = msg;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                using (SmtpClient smtpClient = new SmtpClient("mail.tiscan.hu", 465))
                {
                    smtpClient.EnableSsl = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                    //smtpClient.Send(message.From.ToString(), message.To.ToString(), message.Subject, message.Body);
                    //smtpClient.SendAsync(fromEmail, toEmail, subject, msg, null);
                    smtpClient.Send(fromEmail, recipient, subject, msg);
                }
                Console.WriteLine("after");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Send3()
        {
            SmtpClient client = new SmtpClient("mail.tiscan.hu", 465);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("error@collectioninventory.com", "Smd192192");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("whoever@me.com");
            mailMessage.To.Add("kovacsaurel@gmail.com");
            mailMessage.Body = "body";
            mailMessage.Subject = "subject";
            //client.Send(mailMessage);
            
            var t = new Thread(new ThreadStart(() => client.Send(mailMessage)));
            Console.WriteLine("Send3 after");
            t.IsBackground = false;
            t.Start();
            Console.WriteLine("Send33 after");
        }

    }
}
