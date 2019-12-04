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
    }
}
