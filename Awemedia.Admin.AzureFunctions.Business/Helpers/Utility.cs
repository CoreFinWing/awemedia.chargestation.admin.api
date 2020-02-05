using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public static class Utility
    {
        public static string ConvertUtcToSpecifiedTimeZone(DateTime dateTimeUtc, string timeZoneKey)
        {
            TimeZoneInfo specifiedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneKey);
            DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, specifiedTimeZone);
            return convertedDateTime.ToString("yyyy-MM-dd hh:mm:ss tt");
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
        public static void UploadTextToBlob(string keys)
        {
            double cacheDuration = Convert.ToDouble(Environment.GetEnvironmentVariable("jwksKeysCache"));
            if (!string.IsNullOrEmpty(keys))
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = serviceClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobStorageContainer"));
                container.CreateIfNotExistsAsync();
                CloudBlockBlob blob = container.GetBlockBlobReference(Environment.GetEnvironmentVariable("AWSCognitoFileName"));
                blob.FetchAttributesAsync();
                blob.Properties.CacheControl = "max-age=" + cacheDuration * 60;
                blob.Properties.ContentType = "application/json";
                blob.SetPropertiesAsync();
                blob.Metadata["CreateDate"] = DateTime.Now.ToUniversalTime().ToString();
                blob.Metadata["ExpirationDate"] = DateTime.Now.ToUniversalTime().AddMinutes(cacheDuration).ToString();
                blob.SetMetadataAsync();
                blob.UploadTextAsync(keys);
            }
        }
        public static async System.Threading.Tasks.Task<Dictionary<string, string>> DownloadTextFromBlobAsync()
        {
            Dictionary<string, string> keyValuePairs = null;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobStorageContainer"));
            CloudBlockBlob blob = container.GetBlockBlobReference(Environment.GetEnvironmentVariable("AWSCognitoFileName"));
            if (blob.ExistsAsync().Result)
            {
                await blob.FetchAttributesAsync();
                if (blob.Metadata.Count > 0)
                {
                    var expirationDate = Convert.ToDateTime(blob.Metadata["ExpirationDate"]);

                    keyValuePairs = new Dictionary<string, string>
                    {
                        { "Json", blob.DownloadTextAsync().Result },
                        { "ExpirationDate", expirationDate.ToString() }
                    };
                }
            }
            return keyValuePairs;
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
