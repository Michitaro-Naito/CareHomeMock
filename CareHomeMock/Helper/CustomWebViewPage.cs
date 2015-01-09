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
    }
}