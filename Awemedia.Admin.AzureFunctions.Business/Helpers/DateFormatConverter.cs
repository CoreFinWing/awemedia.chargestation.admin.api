using Newtonsoft.Json.Converters;

namespace Awemedia.Admin.AzureFunctions.Helpers
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
