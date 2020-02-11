using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class EmailModel
    {
        public string Subject { get; set; }
        public string MailRecipientsTo { get; set; }
        public string[] MailRecipientsCc { get; set; }
        public string Content { get; set; }
        public string ExcelReportBase64 { get; set; }
        public string FileName { get; set; }
        public string RecipientName { get; set; }
    }
}
