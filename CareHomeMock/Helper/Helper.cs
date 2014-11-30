using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Helper
{
    public static class Helper
    {
        public static string GenerateRowKey(DateTime dateTime)
        {
            return string.Format("{0:D19}_{1}", (DateTime.MaxValue.Ticks - dateTime.Ticks), Guid.NewGuid());
        }
    }
}