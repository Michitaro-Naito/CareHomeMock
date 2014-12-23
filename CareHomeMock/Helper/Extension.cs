using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace CareHomeMock.Helper
{
    public static class Extension
    {
        public static Dictionary<string, object> ToDictionary(this NameValueCollection collection)
        {
            var dic = new Dictionary<string, object>();
            foreach (string key in collection.AllKeys)
            {
                dic.Add(key, collection[key]);
            }
            return dic;
        }

        public static Dictionary<string, object> ToDictionary(this HttpCookieCollection collection)
        {
            var dic = new Dictionary<string, object>();
            foreach (string key in collection.AllKeys)
            {
                dic.Add(key, collection[key]);
            }
            return dic;
        }

        public static string GetPropertyName<TModel, TItem>(this Expression<Func<TModel, TItem>> expression)
        {
            var member = expression.Body as MemberExpression;
            var propertyInfo = member.Member as System.Reflection.PropertyInfo;
            var propertyName = propertyInfo.Name;
            return propertyInfo.Name;
        }

        /// <summary>
        /// Copies fields and properties to the other.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="commaSeparatedMembers">Like "a,b,c"</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void CopyTo<T>(this T src, ref T dest, string commaSeparatedMembers)
        {
            if (src == null)
                throw new InvalidOperationException("src must not be null.");
            if (dest == null)
                throw new InvalidOperationException("dest must not be null.");

            var members = commaSeparatedMembers.Split(',');
            foreach (var m in members)
            {
                var fieldInfo = typeof(T).GetField(m);
                var propertyInfo = typeof(T).GetProperty(m);

                if (fieldInfo == null && propertyInfo == null)
                    throw new InvalidOperationException("Invalid field or property name: " + m);

                if (fieldInfo != null)
                    fieldInfo.SetValue(dest, fieldInfo.GetValue(src));
                if (propertyInfo != null)
                    propertyInfo.SetValue(dest, propertyInfo.GetValue(src));
            }
            return;
        }

        public static void CopyToTagged<T>(this T src, ref T dest, string tagName)
        {
            var members = typeof(T).GetMembers();
            foreach (var m in members)
            {
                var tagAttr = Attribute.GetCustomAttribute(m, typeof(TagAttribute)) as TagAttribute;
                if (tagAttr == null || !tagAttr.Contains(tagName))
                    continue;
                switch (m.MemberType)
                {
                    case System.Reflection.MemberTypes.Field:
                        var f = m as FieldInfo;
                        f.SetValue(dest, f.GetValue(src));
                        break;

                    case System.Reflection.MemberTypes.Property:
                        var p = m as PropertyInfo;
                        p.SetValue(dest, p.GetValue(src));
                        break;
                }
            }
        }

        /// <summary>
        /// Converts UTC to JST(Japan Standard Time).
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static DateTime ToJst(this DateTime utc)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            var jst = TimeZoneInfo.ConvertTimeFromUtc(utc, timeZone);
            return jst;
            //Console.WriteLine("UTC: " + utc.ToString("G") + " -> JST: " + jst2.ToString("G"));
        }

        /// <summary>
        /// Converts UTC DateTime to JST string like "2014/1/1 0:00:00 +000" => "2014/1/1 9:00"
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static string ToJstString(this DateTime utc)
        {
            var jst = utc.ToJst();
            return jst.ToString("yyyy/MM/dd h:mm");
        }

        /// <summary>
        /// Converts UTC DateTime to JST string like "2014/1/1 0:00:00 +000" => "2014/1/1"
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static string ToJstDateString(this DateTime utc)
        {
            var jst = utc.ToJst();
            return jst.ToString("yyyy/MM/dd");
        }

        public static string ToJstYearString(this DateTime utc)
        {
            var jst = utc.ToJst();
            return jst.ToString("yyyy");
        }

        public static string ToJstDateString(this DateTime? utc)
        {
            if (utc == null)
                return "";
            return utc.Value.ToJstDateString();
        }
    }
}