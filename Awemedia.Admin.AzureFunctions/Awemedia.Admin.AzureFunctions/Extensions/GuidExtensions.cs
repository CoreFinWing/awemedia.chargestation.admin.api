using System;
using System.Text;
using System.Security.Cryptography;

namespace Awemedia.Chargestation.AzureFunctions.Extensions
{
    public static class GuidExtensions
    {
        public static Guid StringToGuid(this string str)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(str));
            return new Guid(hash);
        }

        public static string GuidToString(this Guid guid)
        {
            byte[] reversedguid = guid.ToByteArray();
            return Encoding.Default.GetString(reversedguid);
        }
    }
}
