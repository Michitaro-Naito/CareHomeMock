using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Map Search
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CareHomeSearch()
        {
            return View();
        }

        public ActionResult CareManagerSearch()
        {
            return View();
        }

        public ActionResult CareHomeInfo_BasicInfo(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.Find(id);
            if (careHome == null)
                return HttpNotFound();
            return View(careHome);
        }

        public ActionResult CareHomeInfo_CareManagers(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.Find(id);
            if (careHome == null)
                return HttpNotFound();
            return View(careHome);
        }

        public ActionResult CareHomeInfo_Media(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.Find(id);
            if (careHome == null)
                return HttpNotFound();
            return View(careHome);
        }

        public ActionResult CareHomeInfo_AccessMap(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.Find(id);
            if (careHome == null)
                return HttpNotFound();
            return View(careHome);
        }

        public ActionResult CareManagerInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSelectOptions()
        {
            var prefectures = db.Areas
                .GroupBy(a => a.PrefectureCode)
                .Select(a => new { id = a.FirstOrDefault().PrefectureCode, name = a.FirstOrDefault().PrefectureName })
                .OrderBy(p => p.id)
                .ToList();

            var cities = db.Areas
                .Select(a => new { id = a.CityCode, name = a.CityName })
                .OrderBy(c => c.id)
                .ToList();

            var companyTypes = Enum.GetNames(typeof(CompanyType))
                .Select(n => new { id = Enum.Parse(typeof(CompanyType), n), name = n })
                .ToList();

            return Json(new { prefectures = prefectures, cities = cities, companyTypes = companyTypes });
        }

        [HttpPost]
        public ActionResult GetCareHomes(int? prefectureCode, int? cityCode, CompanyType? companyType, string keyword, int? page)
        {
            // Active
            var q = db.CareHomes.Where(h => !h.Deactivated);

            // Prefecture
            if (prefectureCode != null)
                q = q.Where(h => h.Area.PrefectureCode == prefectureCode.Value);

            // City
            if (cityCode != null)
                q = q.Where(h => h.Area.CityCode == cityCode.Value);

            // CompanyType
            if (companyType != null)
                q = q.Where(h => h.CompanyType == companyType.Value);

            // Keyword
            if (keyword != null)
                q = q.Where(h => h.CompanyName.Contains(keyword) || h.Messages.Contains(keyword));

            // Paging
            var offset = 0;
            if (page != null)
                offset = 50 * page.Value;
            var careHomes = q.OrderBy(h => h.CareHomeId).Skip(offset).Take(50).ToList()
                .Select(h => new
                {
                    CareHomeId = h.CareHomeId,
                    CompanyName = h.CompanyName,
                    Address = h.Address,
                    Years = 123,
                    CareManagerCount = h.CareManagers.Count,
                    ReviewCount = 456,
                    Rating = 4.5
                }).ToList();

            return Json(careHomes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}