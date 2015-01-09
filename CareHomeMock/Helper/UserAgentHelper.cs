using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CareHomeMock.Helper
{
    public static class UserAgentHelper
    {
        public static bool IsMobile(string userAgentString)
        {
            if (userAgentString == null)
                return false;

            var regex = new Regex(@"iPhone|iPod|Windows Phone|BlackBerry");
            if (regex.IsMatch(userAgentString))
                // iPhone, iPod, Windows Phone or BlackBerry.
                return true;

            if (userAgentString.Contains("Android") && userAgentString.Contains("Mobile"))
                // Android Mobile. (Not Android Tablet)
                return true;

            return false;
        }
    }
}