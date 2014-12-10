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

            // CareHomeInfo_BasicInfo
            routes.MapRoute(
                name: "CareHomeInfo_BasicInfo",
                url: "事業所/{code}/{name}/基本情報",
                defaults: new { controller = "Home", action = "CareHomeInfo_BasicInfo", code = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            // CareHomeInfo_CareManagers
            routes.MapRoute(
                name: "CareHomeInfo_CareManagers",
                url: "事業所/{code}/{name}/ケアマネ一覧",
                defaults: new { controller = "Home", action = "CareHomeInfo_CareManagers", code = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            // CareHomeInfo_Media
            routes.MapRoute(
                name: "CareHomeInfo_Media",
                url: "事業所/{code}/{name}/写真と動画",
                defaults: new { controller = "Home", action = "CareHomeInfo_Media", code = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            // CareHomeInfo_AccessMap
            routes.MapRoute(
                name: "CareHomeInfo_AccessMap",
                url: "事業所/{code}/{name}/周辺地図",
                defaults: new { controller = "Home", action = "CareHomeInfo_AccessMap", code = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            // CareHomeInfo_BasicInfo2
            routes.MapRoute(
                name: "CareHomeInfo_BasicInfo2",
                url: "事業所/{code}",
                defaults: new { controller = "Home", action = "CareHomeInfo_BasicInfo", code = UrlParameter.Optional }
            );

            // Application_Send
            routes.MapRoute(
                name: "Application_Send",
                url: "事業所登録申請/{careHomeCode}",
                defaults: new { controller = "Application", action = "Send", careHomeCode = UrlParameter.Optional }
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

            // MediaFiles (Virtual directory like feature. Uploaded by Users.)
            routes.MapRoute(
                name: "MediaFiles",
                url: "MediaFiles/{fileName}",
                defaults: new { controller = "MediaFile", action = "Download", fileName = UrlParameter.Optional }
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
