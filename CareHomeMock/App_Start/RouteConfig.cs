﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CareHomeMock
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.RouteExistingFiles = true;

            // CareHomeSearch
            routes.MapRoute(
                name: "CareHomeSearch",
                url: "事業所検索",
                defaults: new { controller = "Home", action = "CareHomeSearch" }
            );

            // Files (Virtual directory like feature)
            routes.MapRoute(
                name: "Files",
                url: "Files/{fileName}",
                defaults: new { controller = "File", action = "Download", fileName = UrlParameter.Optional }
            );

            // Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
