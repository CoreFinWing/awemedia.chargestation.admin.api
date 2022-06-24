using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Helpers
{
    public static class RandomPassword
    {
        public static string GetRandomPassword(int length)
        {
            byte[] rgb = new byte[length];
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            rngCrypt.GetBytes(rgb);
            return Convert.ToBase64String(rgb);
        }
    }
}
