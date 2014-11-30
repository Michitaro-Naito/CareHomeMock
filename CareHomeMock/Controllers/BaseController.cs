using CareHomeMock.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Helper;

namespace CareHomeMock.Controllers
{
    public class BaseController : Controller
    {
        protected void Log(LogType level, string userId, string action, object data)
        {
            var log = new Log()
            {
                // Layer 4
                IpAddress = Request.UserHostAddress,

                // Layer 7
                HttpMethod = Request.HttpMethod,
                Url = Request.Url.ToString(),
                ContentLength = Request.ContentLength,
                Cookies = JsonConvert.SerializeObject(Request.Cookies.ToDictionary(), Formatting.Indented),
                QueryString = JsonConvert.SerializeObject(Request.QueryString.ToDictionary(), Formatting.Indented),
                Form = JsonConvert.SerializeObject(Request.Form.ToDictionary(), Formatting.Indented),

                // Application Layer
                LogType = level,
                UserId = userId,
                Action = action,
                JsonData = JsonConvert.SerializeObject(data, Formatting.Indented)
            };
            TableHelper<Log>.Table.Insert(log);
        }
	}
}