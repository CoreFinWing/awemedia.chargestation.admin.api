using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Resolver;
using AzureFunctions.Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public static class ReportFunctions
    {
        [FunctionName("ReportFunctions")]
        public static async System.Threading.Tasks.Task ExportToExcelAsync([TimerTrigger("%ReportScheduleExpression%", RunOnStartup = false)]TimerInfo myTimer, ILogger log, [Inject]IChargeSessionService _chargeSessionService, [Inject]IEmailService _emailService)
        {
            var listToExport = _chargeSessionService.GetSuccessfulSessions();
            var blobName = Environment.GetEnvironmentVariable("WeeklyReportExcelFileName");
            await Utility.UploadExcelStreamToBlob(listToExport, blobName);
            var stream = Utility.DownloadExcelStreamFromBlob().Result;
            var base64Array = Convert.ToBase64String(stream.ToArray());
            var toDate = DateTime.Now.ToUniversalTime();
            var fromDate = DateTime.Now.ToUniversalTime().AddDays(-7);
            EmailModel emailModel = new EmailModel
            {
                Content = " Paid Sessions Report from <strong>" + fromDate + "</strong>  to <strong> " + toDate + "</strong> ",
                ExcelReportBase64 = base64Array,
                MailRecipientsTo = Environment.GetEnvironmentVariable("MailRecipientsTo"),
                Subject = "Paid Sessions Report from " + fromDate + "  to " + toDate ,
                FileName ="Weekly_Paid_Sessions_Report.xlsx"
            };
            await _emailService.SendEmailAsync(emailModel);
        }
    }
}
