using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

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

        public static SelectList MediaFileTypes
        {
            get
            {
                var types = new[]
                {
                    new { id = MediaFile.MediaFileType.Image, name = "画像" },
                    new { id = MediaFile.MediaFileType.Youtube, name = "Youtube動画" }
                };
                return new SelectList(types, "id", "name");
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

        /*public static PropertyInfo GetPropertyInfo<TSource, TProperty>(
            TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }*/
    }
}