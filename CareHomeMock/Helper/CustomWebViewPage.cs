using CareHomeMock.Helper;
using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CareHomeMock
{
    public class CustomWebViewPage<TModel> : WebViewPage<TModel>
    {
        public bool IsMobile
        {
            get
            {
                return UserAgentHelper.IsMobile(Request.UserAgent);
            }
        }

        public override void Execute()
        {

        }

        public string GetAbsoluteUrl(string relativeUrl)
        {
            return string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, relativeUrl);
        }
    }
}