using CareHomeMock.Helper;
using CareHomeMock.Helper.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CareHomeMock
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Model Validation (Globalize PropertyName and ErrorMessage)
            //ModelMetadataProviders.Current = new CustomModelMetadataProvider();
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangeAttribute), typeof(CustomRangeAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RegularExpressionAttribute), typeof(CustomRegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(CustomRequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(CustomStringLengthAttributeAdapter));
        }

        /// <summary>
        /// Returns string to identify a cache.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="custom">like "$IsMobile;foo;bar"</param>
        /// <returns>like "true_"</returns>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            var tokens = custom.Split(';');
            var strs = new List<string>();
            foreach (var token in tokens)
            {
                switch (token)
                {
                    case "$IsMobile":
                        strs.Add(UserAgentHelper.IsMobile(context.Request.UserAgent).ToString());
                        break;

                    default:
                        break;
                }
            }
            var str = string.Join("_", strs);
            return str;
        }
    }
}
