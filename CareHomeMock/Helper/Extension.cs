using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
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
    }
}