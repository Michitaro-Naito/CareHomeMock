using CareHomeMock.Models;
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

        public static List<Rating> Ratings
        {
            get
            {
                var ratings = new List<Rating>();
                ratings.Add(new Rating { RatingId = 5, Label = "5. すすめる" });
                ratings.Add(new Rating { RatingId = 4, Label = "4. まあすすめる" });
                ratings.Add(new Rating { RatingId = 3, Label = "3. どちらとも言えない" });
                ratings.Add(new Rating { RatingId = 2, Label = "2. あまりすすめない" });
                ratings.Add(new Rating { RatingId = 1, Label = "1. すすめない" });
                return ratings;
            }
        }

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