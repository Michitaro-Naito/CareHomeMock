﻿using CareHomeMock.Models;
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

        public static SelectList GetBirthdayYears(DateTime selected)
        {
            // 1920 to 15 years ago
            var years = new List<int>();
            var start = 1920;
            var end = DateTime.UtcNow.Year - 15;
            for (var n = start; n <= end; n++)
            {
                years.Add(n);
            }
            var selectedValue = selected.Year;
            return new SelectList(years.Select(y => new { id = y, name = y + "年" }), "id", "name", selectedValue);
        }

        public static SelectList GetBirthdayMonths(DateTime selected)
        {
            var months = new List<int>();
            for (var n = 1; n <= 12; n++)
                months.Add(n);
            var selectedValue = selected.Month;
            return new SelectList(months.Select(m => new { id = m, name = m + "月" }), "id", "name", selectedValue);
        }

        public static SelectList GetBirthdayDays(DateTime selected)
        {
            var days = new List<int>();
            for (var n = 1; n <= 31; n++)
                days.Add(n);
            var selectedValue = selected.Day;
            return new SelectList(days.Select(m => new { id = m, name = m + "日" }), "id", "name", selectedValue);
        }

        public static SelectList GetLicensedYears(DateTime selected)
        {
            // 1998 to 1 year later
            var years = new List<int>();
            var start = 1998;
            var end = DateTime.UtcNow.Year + 1;
            for (var n = start; n <= end; n++)
            {
                years.Add(n);
            }
            var selectedValue = selected.Year + "/12/31";
            return new SelectList(years.Select(y => new { id = y + "/12/31", name = y + "年" }), "id", "name", selectedValue);
        }

        /*class Foo
        {
            public int a, b, c;
        }
        public static void CopyTo<T>(ref T dest, T src, string commaSeparatedMembers)
        {
            var members = commaSeparatedMembers.Split(',');
            foreach (var m in members)
            {
                var fieldInfo = typeof(T).GetField(m);
                var propertyInfo = typeof(T).GetProperty(m);

                if (fieldInfo == null && propertyInfo == null)
                    throw new InvalidOperationException("Invalid field or property name: " + m);

                if(fieldInfo != null)
                    fieldInfo.SetValue(dest, fieldInfo.GetValue(src));
                if (propertyInfo != null)
                    propertyInfo.SetValue(dest, propertyInfo.GetValue(src));
            }
            return;
        }*/

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