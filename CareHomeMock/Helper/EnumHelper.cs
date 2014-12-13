using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Helper
{
    public static class EnumHelper<T>
    {
        public static SelectList GetSelectList(T selectedValue)
        {
            var items = Enum.GetNames(typeof(T)).Select(name => new { id = Enum.Parse(typeof(T), name), name = name });
            return new SelectList(items, "id", "name", selectedValue);
            //ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", careManager.CareHomeId);
        }
    }
}