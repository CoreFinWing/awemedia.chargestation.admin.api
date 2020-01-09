using Awemedia.Admin.AzureFunctions.Business.Models;
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
    }
}
