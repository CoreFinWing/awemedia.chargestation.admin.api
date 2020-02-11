using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _sendGridKey;
        private readonly string _fromEmailAddress;
        public EmailService()
        {
            _sendGridKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            _fromEmailAddress = Environment.GetEnvironmentVariable("FromEmailAddress");
        }
        public async System.Threading.Tasks.Task SendEmailAsync(EmailModel emailModel)
        {
            var client = new SendGridClient(_sendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_fromEmailAddress),
                Subject = emailModel.Subject,
                HtmlContent = emailModel.Content,
            };
            msg.AddTo(new EmailAddress(emailModel.MailRecipientsTo));
            msg.AddAttachment(emailModel.FileName, emailModel.ExcelReportBase64);
            await client.SendEmailAsync(msg);
        }
    }
}
