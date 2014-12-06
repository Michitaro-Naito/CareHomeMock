using System;
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

            // CareManagerSearch
            routes.MapRoute(
                name: "CareManagerSearch",
                url: "ケアマネ検索",
                defaults: new { controller = "Home", action = "CareManagerSearch" }
            );

            // CareManagerInfo
            routes.MapRoute(
                name: "CareManagerInfo",
                url: "ケアマネ/{id}/{careHomeName}/{careManagerName}",
                defaults: new { controller = "Home", action = "CareManagerInfo", id = UrlParameter.Optional, careHomeName = UrlParameter.Optional, careManagerName = UrlParameter.Optional }
            );

            // PostReview
            routes.MapRoute(
                name: "PostReview",
                url: "評価する/{id}",
                defaults: new { controller = "Home", action = "PostReview", id = UrlParameter.Optional }
            );

            // QRCode
            routes.MapRoute(
                name: "QRCode",
                url: "QRCode",
                defaults: new { controller = "Home", action = "QRCode" }
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
