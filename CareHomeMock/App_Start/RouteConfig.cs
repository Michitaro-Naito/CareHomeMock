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

            routes.MapRoute(
                name: "StaticPage.Details",
                url: "ページ/{id}",
                defaults: new { controller = "StaticPage", action = "Details", id = UrlParameter.Optional }
            );



            // ----- Admin -----

            routes.MapRoute(
                name: "Admin",
                url: "管理者メニュー",
                defaults: new { controller = "Home", action = "AdminMenu" }
            );

            routes.MapRoute(
                name: "CareHome.DownloadCareHomes.csv",
                url: "CareHome/DownloadCareHomes.csv",
                defaults: new { controller = "CareHome", action = "DownloadCareHomes" }
            );

            routes.MapRoute(
                name: "CareHome.DownloadCareManagers.csv",
                url: "CareHome/DownloadCareManagers.csv",
                defaults: new { controller = "CareHome", action = "DownloadCareManagers" }
            );



            // ----- CareHome -----

            routes.MapRoute(
                name: "CareHomeMenu",
                url: "事業所管理/{code}",
                defaults: new { controller = "Home", action = "CareHomeMenu", code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CareManagerIndex",
                url: "事業所管理/{code}/ケアマネ",
                defaults: new { controller = "CareManager", action = "Index", code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CareManagerEdit",
                url: "事業所管理/{code}/ケアマネ/編集/{careManagerId}",
                defaults: new { controller = "CareManager", action = "Edit", code = UrlParameter.Optional, careManagerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CareManager.Delete",
                url: "事業所管理/{code}/ケアマネ/削除/{careManagerId}",
                defaults: new { controller = "CareManager", action = "Delete", code = UrlParameter.Optional, careManagerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MediaFile.Index",
                url: "事業所管理/{code}/写真動画",
                defaults: new { controller = "MediaFile", action = "Index", code = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MediaFile.Upload",
                url: "事業所管理/{code}/写真動画/編集/{mediaFileId}",
                defaults: new { controller = "MediaFile", action = "Upload", code = UrlParameter.Optional, mediaFileId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MediaFile.Delete",
                url: "事業所管理/{code}/写真動画/削除/{mediaFileId}",
                defaults: new { controller = "MediaFile", action = "Delete", code = UrlParameter.Optional, mediaFileId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CareHome.EditAdditionalInfo",
                url: "事業所管理/{code}/追加情報編集",
                defaults: new { controller = "CareHome", action = "EditAdditionalInfo", code = UrlParameter.Optional }
            );




            // ----- CareManager -----

            routes.MapRoute(
                name: "CareManagerMenu",
                url: "ケアマネ管理/{careManagerId}",
                defaults: new { controller = "Home", action = "CareManagerMenu", careManagerId = UrlParameter.Optional }
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
