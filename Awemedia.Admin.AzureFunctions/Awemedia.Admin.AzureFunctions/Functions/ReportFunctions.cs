using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Awemedia.Admin.AzureFunctions.Functions
{
    public static class ReportFunctions
    {
        [FunctionName("ReportFunctions")]
        public static void ExportToExcel([TimerTrigger("0 */2 * * * *", RunOnStartup = false)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var fileNameSuffix = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }
    }
}
