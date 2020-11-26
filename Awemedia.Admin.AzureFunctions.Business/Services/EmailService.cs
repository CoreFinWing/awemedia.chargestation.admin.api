using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _sendGridKey;
        private readonly string _fromEmailAddress;
        public EmailService()
        {
            _sendGridKey = Environment.GetEnvironmentVariable("email_sendgrid_api_key");
            _fromEmailAddress = Environment.GetEnvironmentVariable("reports_sender_email");
        }
        public async System.Threading.Tasks.Task SendEmailAsync(EmailModel emailModel)
        {
            var client = new SendGridClient(_sendGridKey);
            var emailRecipientsTo = emailModel.MailRecipientsTo.Split(',').ToList();
            List<EmailAddress> recipients = new List<EmailAddress>();
            if(emailRecipientsTo.Any())
            {
                foreach (var recipient in emailRecipientsTo)
                {
                    recipients.Add(new EmailAddress(recipient));
                }
            }
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_fromEmailAddress),
                Subject = emailModel.Subject,
                HtmlContent = emailModel.Content,
                Personalizations = new List<Personalization>
                {
                     new Personalization
                     {
                          Tos = recipients
                     }
                 }
            };
            msg.AddAttachment(emailModel.FileName, emailModel.ExcelReportBase64);
            await client.SendEmailAsync(msg);
        }
    }
}
