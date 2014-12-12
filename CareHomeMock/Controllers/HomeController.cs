using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// Redirects any User to the right menu.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult GotoMenu()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("AdminMenu");

            if (CurrentUser.CareHomes.Count > 0)
            {
                var home = CurrentUser.CareHomes.First();
                return RedirectToAction("CareHomeMenu", new { code = home.CareHomeCode });
            }

            if (CurrentUser.CareManager.Count > 0)
            {
                var careManager = CurrentUser.CareManager.First();
                return RedirectToAction("CareManagerMenu", new { careManagerId = careManager.CareManagerId });
            }

            return HttpNotFound();
        }

        public ActionResult AdminMenu()
        {
            return View();
        }

        public ActionResult CareHomeMenu()
        {
            return View();
        }

        public ActionResult CareManagerMenu()
        {
            return View();
        }

        /// <summary>
        /// Map Search
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration=300)]
        public ActionResult CareHomeSearch()
        {
            return View();
        }

        public ActionResult CareManagerSearch()
        {
            return View();
        }

        public ActionResult CareHomeInfo_BasicInfo(string code, string name)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();
            if (name != careHome.CompanyName)
                return RedirectToAction("CareHomeInfo_BasicInfo", new { code = code, name = careHome.CompanyName });
            return View(careHome);
        }

        public ActionResult CareHomeInfo_CareManagers(string code)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();
            return View(careHome);
        }

        public ActionResult CareHomeInfo_Media(string code)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();
            return View(careHome);
        }

        public ActionResult CareHomeInfo_AccessMap(string code)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
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
        public ActionResult GetReviews(int? id, string afterThisRowKey)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var table = TableHelper<Review>.Table;
            var idString = id.Value.ToString();
            var reviews = table.CreateQuery<Review>().Where(r => r.PartitionKey == idString && r.RowKey.CompareTo(afterThisRowKey) > 0).Take(50).ToList()
                .Select(r => new {
                    RowKey = r.RowKey,
                    Created = r.Created,
                    ReviewerType = r.ReviewerType.ToString(),
                    Rating = r.Rating,
                    Comment = r.Comment,
                    IpAddress = Helper.Helper.GetMd5Hash(r.IpAddress),
                    Reply = r.Reply
                });

            return Json(new { reviews = reviews });
        }

        public ActionResult PostReview(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var careManager = db.CareManagers.Find(id);
            if (careManager == null)
                return HttpNotFound();

            ViewBag.Rating = new SelectList(Helper.Helper.Ratings, "RatingId", "Label");
            return View(new HomePostReviewVM(){ CareManagerId = id.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostReview(HomePostReviewVM model)
        {
            var now = DateTime.UtcNow;
            var otp = db.Otps.FirstOrDefault(o => o.CareManagerId == model.CareManagerId
                && o.VerificationCode == model.Otp
                && o.Expires > now);
            if(otp == null)
                ModelState.AddModelError("Otp", "無効なOTPです。");
            if (ModelState.IsValid)
            {
                // Removes Otp
                db.Otps.Remove(otp);
                db.ReviewRatings.Add(new ReviewRating() { CareManagerId = otp.CareManagerId, Rating = model.Rating });
                db.SaveChanges();

                // Updates TableStorage
                AddReview(otp.CareManagerId, otp.ReviewerType, model.Rating, model.Message);

                // Calculates CareManager.Rating and CareManager.ReviewCount (Cached value to display)
                var careManager = db.CareManagers.Find(otp.CareManagerId);
                var oneYearAgo = DateTime.UtcNow.AddYears(-1);
                var ratings = careManager.ReviewRatings.Where(r => r.Created > oneYearAgo);
                var sum = ratings.Sum(r => r.Rating);
                var count = ratings.Count();
                careManager.TotalRating = sum;
                careManager.ReviewsCount = count;
                careManager.Rating = (double)sum / count;

                // Calculates CareHome.Rating and CareHome.ReviewCount
                var home = careManager.CareHome;
                var homeSum = home.CareManagers.Sum(m => m.TotalRating);
                var homeCount = home.CareManagers.Sum(m => m.ReviewsCount);
                home.ReviewCount = homeCount;
                home.Rating = homeSum / count;

                // Removes old ReviewRatings
                var ratingsToRemove = db.ReviewRatings.Where(r => r.Created <= oneYearAgo).ToList();
                db.ReviewRatings.RemoveRange(ratingsToRemove);

                // Updates SQL
                db.SaveChanges();
            }
            ViewBag.Rating = new SelectList(Helper.Helper.Ratings, "RatingId", "Label");
            return View(model);
        }

        [HttpPost]
        [OutputCache(Duration=300)]
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
                new { id = "Years", name = "運営年数" },
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
                .Select(n => {
                    var name = n;
                    var regex = new Regex(@"Range(?<num>[0-9]+)");
                    name = regex.Replace(name, match => {
                        switch (match.Groups["num"].Value)
                        {
                            case "10":
                                return "20歳未満";
                            case "90":
                                return "90歳以上";
                            default:
                                return match.Groups["num"].Value + "代";
                        }
                    });
                    return new { id = Enum.Parse(typeof(AgeRange), n), name = name };
                })
                .ToList();

            var licenses = db.Licenses
                .Select(l => new { id = l.LicenseId, name = l.Name })
                .OrderBy(l => l.id)
                .ToList();

            var sortFields = new[]{
                new { id = "ReviewCount", name = "評価件数" },
                new { id = "Rating", name = "評価得点" },
                new { id = "Years", name = "経験年数" }
            };

            return Json(new
            {
                prefectures = prefectures,
                cities = cities,
                genders = genders,
                ageRanges = ageRanges,
                licenses = licenses,
                sortFields = sortFields
            });
        }

        [HttpPost]
        [OutputCache(Duration = 300, VaryByParam = "prefectureCode;cityCode;companyType;keyword;page;sortField;descending")]
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
                case "Years":
                    if (descending)
                        rows = q.OrderBy(h => h.Established);
                    else
                        rows = q.OrderByDescending(h => h.Established);
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
                    CareHomeCode = h.CareHomeCode,
                    CompanyName = h.CompanyName,
                    Address = h.Area.PrefectureName + h.Area.CityName + h.Address,
                    Years = h.Years,
                    CareManagerCount = h.CareManagers.Count,
                    ReviewCount = h.ReviewCount,
                    Rating = h.Rating,
                    Messages = h.Messages,
                    MediaFileDataId = h.MediaFileDataId
                }).ToList();

            return Json(new { count = count, careHomes = careHomes });
        }

        public ActionResult GetCareManagers(int? prefectureCode, int? cityCode, Gender? gender, AgeRange? ageRange, bool? allowNewPatient, int? licenseId, string sortField, bool descending, int? page)
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
            if (ageRange != null)
            {
                var min = new DateTime(1, 1, 1);
                var max = new DateTime(3000, 1, 1);
                switch (ageRange.Value)
                {
                    case AgeRange.Range10:
                        min = DateTime.UtcNow.AddYears(-20);
                        break;
                    default:
                        min = DateTime.UtcNow.AddYears(-10 * ((int)ageRange.Value + 1));
                        max = DateTime.UtcNow.AddYears(-10 * (int)ageRange.Value);
                        break;
                    case AgeRange.Range90:
                        max = DateTime.UtcNow.AddYears(-90);
                        break;
                }
                mq = mq.Where(m => m.Birthday > min && m.Birthday <= max);
            }

            // AllowNewPatient
            if (allowNewPatient != null)
                mq = mq.Where(m => m.AllowNewPatient == allowNewPatient.Value);

            // Licenses
            if (licenseId != null)
                mq = mq.Where(m => m.CareManagerLicenses.Any(l => l.LicenseId == licenseId.Value));

            // Sort
            IQueryable<CareManager> rows;
            switch (sortField)
            {
                case "ReviewCount":
                default:
                    if (descending)
                        rows = mq.OrderByDescending(m => m.ReviewsCount);
                    else
                        rows = mq.OrderBy(m => m.ReviewsCount);
                    break;
                case "Rating":
                    if (descending)
                        rows = mq.OrderByDescending(m => m.Rating);
                    else
                        rows = mq.OrderBy(m => m.Rating);
                    break;
                case "Years":
                    if (descending)
                        rows = mq.OrderBy(m => m.Licensed);
                    else
                        rows = mq.OrderByDescending(m => m.Licensed);
                    break;
            }

            // Paging
            var limit = 50;
            var offset = 0;
            if (page != null)
                offset = limit * page.Value;
            var count = rows.Count();

            var careManagers = rows.Skip(offset).Take(limit).ToList().Select(m => new {
                CareHomeId = m.CareHomeId,
                CareHomeName = m.CareHome.CompanyName,
                CareManagerId = m.CareManagerId,
                CareManagerName = m.Name,
                Years = m.Years,
                ReviewCount = m.ReviewsCount,
                Rating = m.Rating,
                MediaFileDataId = m.MediaFileDataId
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
    }
}