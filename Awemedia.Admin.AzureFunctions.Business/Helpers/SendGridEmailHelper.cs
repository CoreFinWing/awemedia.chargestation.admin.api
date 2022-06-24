using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public class EmailSenderConfig
    {
        public string APIKey { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
    }
    public static class SendGridEmailHelper
    {
        public const string USER_NAME = "apikey";
        public static string SendEmail(EmailSenderConfig config)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    config.FromName,
                   config.FromEmail
                ));
                message.To.Add(new MailboxAddress(
                    config.ToName,
                    config.ToEmail
                ));
                message.Subject = config.Subject;
                var bodyBuilder = new BodyBuilder();
                if (!string.IsNullOrEmpty(config.TextBody))
                {
                    bodyBuilder.TextBody = config.TextBody;
                }
                if (!string.IsNullOrEmpty(config.HtmlBody))
                {
                    bodyBuilder.HtmlBody = config.HtmlBody;
                }
                message.Body = bodyBuilder.ToMessageBody();

                var client = new SmtpClient();
                // SecureSocketOptions.StartTls force a secure connection over TLS
                client.Connect("smtp.sendgrid.net", 587, SecureSocketOptions.StartTls);
                client.Authenticate(
                    userName: USER_NAME,
                    password: config.APIKey
                );

                var log = client.Send(message);
                client.Disconnect(true);
                return log;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
