using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Helper
{
    public class IdNamePair<T>
    {
        public T id { get; set; }
        public string name { get; set; }
    }

    public static class EnumHelper<T>
    {
        /// <summary>
        /// Gets display name from DisplayAttribute if available.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetDisplayName(string name)
        {
            var type = typeof(T);
            var memberInfo = type.GetMember(name);
            var attributes = (DisplayAttribute[])memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length > 0)
                name = attributes[0].GetName();
            return name;
        }

        public static List<IdNamePair<T>> GetIdNamePairs()
        {
            var type = typeof(T);

            var names = Enum.GetNames(type);
            var items = names.Select(n =>
            {
                return new IdNamePair<T>() { id = (T)Enum.Parse(type, n), name = GetDisplayName(n) };
            }).ToList();
            return items;
        }

        public static SelectList GetSelectList(T selectedValue)
        {
            var items = Enum.GetNames(typeof(T)).Select(name => new { id = Enum.Parse(typeof(T), name), name = name });
            return new SelectList(items, "id", "name", selectedValue);
        }
    }
}