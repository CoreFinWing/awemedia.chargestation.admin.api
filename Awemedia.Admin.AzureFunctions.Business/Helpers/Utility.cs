using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.IO;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public static class Utility
    {
        public static string ConvertUtcToSpecifiedTimeZone(DateTime dateTimeUtc, string timeZoneKey)
        {
            TimeZoneInfo specifiedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneKey);
            DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, specifiedTimeZone);
            return convertedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
        }
        public static DateTime ParseStartAndEndDates(BaseSearchFilter userSessionSearchFilter, ref DateTime toDate)
        {
            DateTime fromDate;
            if (!string.IsNullOrEmpty(userSessionSearchFilter.FromDate) && !string.IsNullOrEmpty(userSessionSearchFilter.ToDate))
            {
                fromDate = DateTime.Parse(userSessionSearchFilter.FromDate);
                toDate = DateTime.Parse(userSessionSearchFilter.ToDate);
            }
            else
            {
                fromDate = toDate.AddDays(-30);
            }

            return fromDate;
        }
        public static async System.Threading.Tasks.Task UploadExcelStreamToBlob<T>(System.Collections.Generic.IEnumerable<T> listToExport, string blobName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobStorageContainer"));
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            await blob.DeleteIfExistsAsync();
            using (var excel = new ExcelPackage())
            {
                var worksheet = excel.Workbook.Worksheets.Add("Successful Sessions");
                worksheet.Column(6).Style.Numberformat.Format = "MM/dd/yyyy hh:mm:ss";
                worksheet.Column(7).Style.Numberformat.Format = "MM/dd/yyyy hh:mm:ss";
                worksheet.Column(25).Style.Numberformat.Format = "MM/dd/yyyy hh:mm:ss";
                worksheet.Column(26).Style.Numberformat.Format = "MM/dd/yyyy hh:mm:ss";
                worksheet.Cells.LoadFromCollection(listToExport, true, TableStyles.Medium15);
                byte[] bytes = excel.GetAsByteArray();
                using (Stream stream = new MemoryStream(bytes))
                {
                    await blob.UploadFromStreamAsync(stream);
                }
            }
        }
        public static async System.Threading.Tasks.Task<MemoryStream> DownloadExcelStreamFromBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobStorageContainer"));
            CloudBlockBlob blob = container.GetBlockBlobReference(Environment.GetEnvironmentVariable("WeeklyReportExcelFileName"));
            var stream = new MemoryStream();
            await blob.DownloadToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
