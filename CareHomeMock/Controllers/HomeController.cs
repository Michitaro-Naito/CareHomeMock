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

        public ActionResult CareManagerInfo(int? id, string careHomeName, string careManagerName)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careManager = db.CareManagers.Find(id);
            if (careManager == null)
                return HttpNotFound();
            if (careHomeName != careManager.CareHome.CompanyName || careManagerName != careManager.Name)
                return RedirectToRoute(new { action = "CareManagerInfo", controller = "Home", id = id, careHomeName = careManager.CareHome.CompanyName, careManagerName = careManager.Name });
            return View(careManager);
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

            var sortFields = new[]{
                new { id = "ReviewCount", name = "評価件数" },
                new { id = "Rating", name = "評価得点" },
                new { id = "Established", name = "設立年月日" },
                new { id = "CareManagerCount", name = "ケアマネ人数" }
            };

            return Json(new { prefectures = prefectures, cities = cities, companyTypes = companyTypes, sortFields = sortFields });
        }

        [HttpPost]
        public ActionResult GetSelectOptions_CareManagerSearch()
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

            var genders = Enum.GetNames(typeof(Gender))
                .Select(n => new { id = Enum.Parse(typeof(Gender), n), name = n })
                .ToList();

            var ageRanges = Enum.GetNames(typeof(AgeRange))
                .Select(n => new { id = Enum.Parse(typeof(AgeRange), n), name = n })
                .ToList();

            return Json(new
            {
                prefectures = prefectures,
                cities = cities,
                genders = genders,
                ageRanges = ageRanges
            });
        }

        [HttpPost]
        public ActionResult GetCareHomes(int? prefectureCode, int? cityCode, CompanyType? companyType, string keyword, int? page, string sortField, bool descending = false)
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
            var limit = 50;
            var offset = 0;
            if (page != null)
                offset = limit * page.Value;
            var count = q.Count();
            IQueryable<CareHome> rows;
            switch (sortField)
            {
                case "Established":
                    if (descending)
                        rows = q.OrderByDescending(h => h.Established);
                    else
                        rows = q.OrderBy(h => h.Established);
                    break;

                case "CareManagerCount":
                    if (descending)
                        rows = q.OrderByDescending(h => h.CareManagers.Count);
                    else
                        rows = q.OrderBy(h => h.CareManagers.Count);
                    break;

                case "ReviewCount":
                default:
                    if (descending)
                        rows = q.OrderByDescending(h => h.ReviewCount);
                    else
                        rows = q.OrderBy(h => h.ReviewCount);
                    break;

                case "Rating":
                    if (descending)
                        rows = q.OrderByDescending(h => h.Rating);
                    else
                        rows = q.OrderBy(h => h.Rating);
                    break;
            }
            var careHomes = rows.Skip(offset).Take(limit).ToList().Select(h => new
                {
                    CareHomeId = h.CareHomeId,
                    CompanyName = h.CompanyName,
                    Address = h.Address,
                    Years = h.Years,
                    CareManagerCount = h.CareManagers.Count,
                    ReviewCount = h.ReviewCount,
                    Rating = h.Rating,
                    Messages = h.Messages
                }).ToList();

            return Json(new { count = count, careHomes = careHomes });
        }

        public ActionResult GetCareManagers(int? prefectureCode, int? cityCode, Gender? gender, AgeRange? ageRange, bool? allowNewPatient /* licenses */, int? page)
        {
            // Active
            var q = db.CareHomes.Where(h => !h.Deactivated);

            // Prefecture
            if (prefectureCode != null)
                q = q.Where(h => h.Area.PrefectureCode == prefectureCode.Value);

            // City
            if (cityCode != null)
                q = q.Where(h => h.Area.CityCode == cityCode.Value);

            // CareManagers
            var mq = q.SelectMany(h => h.CareManagers);

            // Gender
            if(gender != null)
                mq = mq.Where(m => m.Gender == gender.Value);

            // AgeRange
            //if(ageRange != null)
            //    careManagers = careManagers.Where(m => m.Birthday)

            // AllowNewPatient
            if (allowNewPatient != null)
                mq = mq.Where(m => m.AllowNewPatient == allowNewPatient.Value);

            // Licenses

            // Paging
            var limit = 50;
            var offset = 0;
            if (page != null)
                offset = limit * page.Value;
            var count = mq.Count();

            var careManagers = mq.OrderBy(m=>m.CareManagerId).Skip(offset).Take(limit).ToList().Select(m => new {
                CareHomeId = m.CareHomeId,
                CareHomeName = m.CareHome.CompanyName,
                CareManagerId = m.CareManagerId,
                CareManagerName = m.Name,
                Years = m.Years,
                ReviewCount = m.ReviewsCount,
                Rating = m.Rating
            }).ToList();

            return Json(new 
            {
                count = count,
                careManagers = careManagers
            });
        }

        public ActionResult QRCode()
        {
            return View();
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