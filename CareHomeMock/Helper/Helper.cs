using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CareHomeMock.Helper
{
    public static class Helper
    {
        public const string SecuritySalt = "DevelopedByAmlitek.com.erai@nojxcoijnSKJiojwieij128h;o";

        public static string GenerateRowKey(DateTime dateTime)
        {
            return string.Format("{0:D19}_{1}", (DateTime.MaxValue.Ticks - dateTime.Ticks), Guid.NewGuid());
        }

        public static string GetMd5Hash(string original)
        {
            using (var crypto = new MD5CryptoServiceProvider())
            {
                var bytesOriginal = Encoding.UTF8.GetBytes(original + SecuritySalt);
                var bytesEncrypted = crypto.ComputeHash(bytesOriginal);
                return Convert.ToBase64String(bytesEncrypted).Replace("=", "");
            }
        }
    }
}