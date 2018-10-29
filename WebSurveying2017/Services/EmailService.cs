using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Configuration;
using SendGrid;

namespace WebSurveying2017.Services
{
    public class EmailService : IIdentityMessageService
    {

        #region Private Fields

        private static string FromAddress;
        private static string strSmtpClient;
        private static string UserID;
        private static string Password;
        private static string SMTPPort;
        private static bool bEnableSSL;

        #endregion

        #region Interface Implementation

        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        #endregion

        #region Send Email Method
        public async Task configSendGridasync(IdentityMessage message)
        {
            GetMailData();
            dynamic MailMessage = new MailMessage();
            MailMessage.From = new MailAddress(FromAddress);
            MailMessage.To.Add(message.Destination);
            MailMessage.Subject = message.Subject;
            MailMessage.IsBodyHtml = true;
            MailMessage.Body = message.Body;

            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Host = strSmtpClient;
            SmtpClient.EnableSsl = bEnableSSL;
            SmtpClient.Port = Int32.Parse(SMTPPort);
            SmtpClient.Credentials = new System.Net.NetworkCredential(UserID, Password);

            try
            {
                try
                {
                    SmtpClient.Send(MailMessage);

                }
                catch (Exception ex)
                {

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i <= ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.StatusCode;
                    if ((status == SmtpStatusCode.MailboxBusy) | (status == SmtpStatusCode.MailboxUnavailable))
                    {
                        System.Threading.Thread.Sleep(5000);
                        SmtpClient.Send(MailMessage);
                    }
                }
            }

        }
        
        /*
            var myMessage = new SendGridMessage();

            myMessage.AddTo("web-jzya1@mail-tester.com");
            myMessage.From = new System.Net.Mail.MailAddress("webanketiranje@gmail.com", "Taiseer Joudeh");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserID"],
                                                    ConfigurationManager.AppSettings["Password"]);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
            */
            #endregion
        
            #region Get Email provider data From Web.config file
            private static void GetMailData()
        {
            FromAddress = System.Configuration.ConfigurationManager.AppSettings.Get("FromAddress");
            strSmtpClient = System.Configuration.ConfigurationManager.AppSettings.Get("SmtpClient");
            UserID = System.Configuration.ConfigurationManager.AppSettings.Get("UserID");
            Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            //ReplyTo = System.Configuration.ConfigurationManager.AppSettings.Get("ReplyTo");
            SMTPPort = System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort");
            if ((System.Configuration.ConfigurationManager.AppSettings.Get("EnableSSL") == null))
            {
            }
            else
            {
                if ((System.Configuration.ConfigurationManager.AppSettings.Get("EnableSSL").ToUpper() == "YES"))
                {
                    bEnableSSL = true;
                }
                else
                {
                    bEnableSSL = false;
                }
            }
        }
        #endregion

    }
}