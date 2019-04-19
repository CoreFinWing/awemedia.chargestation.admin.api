using System;
using System.Text;

namespace Awemedia.Chargestation.Api.Helpers
{
    public static class Base64Extensions
    {
        public static string StringToBase64(this string str)
        {
            return Convert.ToBase64String(
                                Encoding.GetEncoding("ISO-8859-1")
                                  .GetBytes(s: "mobileapp" + ":" + "dGVzdA==")
                                );
        }

        public static string Base64ToString(this string encodedString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
        }
    }
}
