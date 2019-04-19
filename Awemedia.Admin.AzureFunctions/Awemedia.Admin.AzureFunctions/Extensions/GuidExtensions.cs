using System;
using System.Text;

namespace Awemedia.Chargestation.AzureFunctions.Extensions
{
    public static class GuidExtensions
    {
        public static Guid StringToGuid(this string str,string encodedString)
        {
            byte[] databytes = Encoding.Default.GetBytes(encodedString);
            Guid guid = new Guid(databytes);
            return guid;
        }

        public static string GuidToString(this string encodedString, Guid guid)
        {
            byte[] reversedguid = guid.ToByteArray();
            return Encoding.Default.GetString(reversedguid);
        }
    }
}
