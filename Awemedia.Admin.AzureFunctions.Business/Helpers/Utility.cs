using Awemedia.Admin.AzureFunctions.Business.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

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
        public static void UploadTextToBlob(string keys)
        {
            if (!string.IsNullOrEmpty(keys))
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = serviceClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobStorageContainer"));
                container.CreateIfNotExistsAsync();
                CloudBlockBlob blob = container.GetBlockBlobReference(Environment.GetEnvironmentVariable("AWSCognitoFileName"));
                blob.Properties.CacheControl = "max-age=3600";
                blob.Properties.ContentType = "application/json";
                blob.SetPropertiesAsync();
                blob.UploadTextAsync(keys);
            }
        }
        public static string DownloadTextFromBlobAsync()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobStorageContainer"));
            CloudBlockBlob blob = container.GetBlockBlobReference(Environment.GetEnvironmentVariable("AWSCognitoFileName"));
            if (blob.ExistsAsync().Result)
            {
                return blob.DownloadTextAsync().Result;
            }
            return string.Empty;
        }
    }
}
