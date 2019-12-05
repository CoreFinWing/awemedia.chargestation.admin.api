using Awemedia.Admin.AzureFunctions.Business.Models;
using System;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public static class Utility
    {
        public static DateTime ConvertUtcToSpecifiedTimeZone(DateTime dateTimeUtc, string timeZoneKey)
        {
            TimeZoneInfo specifiedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneKey);
            DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, specifiedTimeZone);
            return convertedDateTime;
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
