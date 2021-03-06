﻿using CareHomeMock.Models;
using System;
using System.Data;
using System.Data.Entity;
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
        //[OutputCache(Duration=30, VaryByParam="abc", VaryByCustom="$IsMobile")]
        public ActionResult Index()
        {
            return View();
        }

        //[OutputCache(Duration=300)]
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
            if (name != careHome.Name)
                return RedirectToAction("CareHomeInfo_BasicInfo", new { code = code, name = careHome.Name });
            return View(careHome);
        }

        public ActionResult CareHomeInfo_CareManagers(string code, string name)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();
            if (name != careHome.Name)
                return RedirectToAction("CareHomeInfo_CareManagers", new { code = code, name = careHome.Name });
            return View(careHome);
        }

        public ActionResult CareHomeInfo_Media(string code, string name)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();
            if (name != careHome.Name)
                return RedirectToAction("CareHomeInfo_Media", new { code = code, name = careHome.Name });
            return View(careHome);
        }

        public ActionResult CareHomeInfo_AccessMap(string code, string name)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();
            if (name != careHome.Name)
                return RedirectToAction("CareHomeInfo_AccessMap", new { code = code, name = careHome.Name });
            return View(careHome);
        }

        public ActionResult CareManagerInfo(int? id, string careHomeName, string careManagerName)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var careManager = db.CareManagers.Find(id);
            if (careManager == null)
                return HttpNotFound();
            if (careHomeName != careManager.CareHome.Name || careManagerName != careManager.Name)
                return RedirectToRoute(new { action = "CareManagerInfo", controller = "Home", id = id, careHomeName = careManager.CareHome.Name, careManagerName = careManager.Name });
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
                    PartitionKey = r.PartitionKey,
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
            ViewBag.ReviewerType = Helper.EnumHelper<ReviewerType>.GetSelectList(ReviewerType.要介護者本人);
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
                var review = new ReviewRating() { CareManagerId = otp.CareManagerId, Rating = model.Rating };
                db.ReviewRatings.Add(review);
                db.SaveChanges();

                // Updates TableStorage
                AddReview(otp.CareManagerId, model.ReviewerType, model.Rating, model.Message);

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
                home.Rating = homeSum / homeCount;

                // Removes old ReviewRatings
                var ratingsToRemove = db.ReviewRatings.Where(r => r.Created <= oneYearAgo).ToList();
                db.ReviewRatings.RemoveRange(ratingsToRemove);

                // Updates SQL
                db.SaveChanges();
                Log(LogType.Others, "ケアマネを評価しました。", new { review.CareManagerId });

                Flash("ケアマネを評価しました。");
                return RedirectToAction("CareManagerInfo", "Home", new { id = model.CareManagerId });
            }
            ViewBag.Rating = new SelectList(Helper.Helper.Ratings, "RatingId", "Label");
            ViewBag.ReviewerType = Helper.EnumHelper<ReviewerType>.GetSelectList(model.ReviewerType);
            return View(model);
        }

        [HttpPost]
        //[OutputCache(Duration=300)]
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

            var companyTypes = Helper.EnumHelper<CompanyType>.GetIdNamePairs();

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
                            case "70":
                                return "70代以上";
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
        //[OutputCache(Duration = 300, VaryByParam = "prefectureCode;cityCode;companyType;keyword;page;sortField;descending")]
        public ActionResult GetCareHomes(int? prefectureCode, int? cityCode, CompanyType? companyType, string keyword, int? page, string sortField, bool descending = false)
        {
            var start = DateTime.UtcNow;

            // Active
            var q = db.CareHomes.Where(h => !h.Deactivated).Include(h => h.Area).Include(h => h.CareManagers);

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
            if (!string.IsNullOrEmpty(keyword))
                q = q.Where(h => h.Name.Contains(keyword) || h.Traits.Contains(keyword));

            // Order
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

            // Paging
            var limit = 50;
            var offset = 0;
            if (page != null)
                offset = limit * page.Value;
            var count = q.Count();
            var countRegistered = q.Count(h => h.UserId != null);
            var countNotRegistered = count - countRegistered;

            // Takes registered first and not registered second.
            var careHomesRegistered = new List<CareHome>();
            var careHomesNotRegistered = new List<CareHome>();
            var skipRegistered = offset;
            var takeRegistered = Math.Min(limit, countRegistered - skipRegistered);
            var skipNotRegistered = Math.Max(0, offset - countRegistered);
            var takeNotRegistered = limit - takeRegistered;

            Debug.WriteLine("{0} ms", (DateTime.UtcNow - start).TotalMilliseconds);

            if(takeRegistered > 0)
            {
                careHomesRegistered = rows.Where(h => h.UserId != null)
                    .Skip(skipRegistered).Take(takeRegistered).ToList();
            }
            if(takeNotRegistered > 0)
            {
                careHomesNotRegistered = rows.Where(h => h.UserId == null)
                    .Skip(skipNotRegistered).Take(takeNotRegistered).ToList();
            }

            Debug.WriteLine("{0} ms", (DateTime.UtcNow - start).TotalMilliseconds);

            var careHomes = careHomesRegistered.Concat(careHomesNotRegistered).Select(h => new
                {
                    CareHomeId = h.CareHomeId,
                    CareHomeCode = h.CareHomeCode,
                    Name = h.Name,
                    Address = h.Area.PrefectureName + h.Area.CityName + h.Address + h.AddressBuilding,
                    Established = h.Established,
                    Years = h.Years,
                    CareManagerCount = h.CareManagers.Count,
                    ReviewCount = h.ReviewCount,
                    Rating = h.Rating,
                    Messages = h.Messages,
                    MediaFileDataId = h.MediaFileDataId
                }).ToList();

            Debug.WriteLine("{0} ms", (DateTime.UtcNow - start).TotalMilliseconds);

            return Json(new { count = count, careHomes = careHomes });
        }

        public ActionResult GetCareManagers(int? prefectureCode, int? cityCode, Gender? gender, AgeRange? ageRange, bool? allowNewPatient, int? licenseId, string sortField, bool descending, int? page, string keyword = null)
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
            var mq = q.SelectMany(h => h.CareManagers).Where(m => m.UserId != null);    // Email verified CareManagers who belongs to these CareHomes.

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
                    default:
                        min = DateTime.UtcNow.AddYears(-10 * ((int)ageRange.Value + 1));
                        max = DateTime.UtcNow.AddYears(-10 * (int)ageRange.Value);
                        break;
                    case AgeRange.Range70:
                        max = DateTime.UtcNow.AddYears(-70);
                        break;
                }
                mq = mq.Where(m => m.Birthday > min && m.Birthday <= max);
            }

            // AllowNewPatient only?
            if (allowNewPatient != null && allowNewPatient.Value)
                mq = mq.Where(m => m.AllowNewPatient == allowNewPatient.Value);

            // Licenses
            if (licenseId != null)
                mq = mq.Where(m => m.CareManagerLicenses.Any(l => l.LicenseId == licenseId.Value));

            // Keyword
            if (!string.IsNullOrEmpty(keyword))
                mq = mq.Where(m => m.Name.Contains(keyword));

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
                CareHomeName = m.CareHome.Name,
                CareManagerId = m.CareManagerId,
                CareManagerName = m.Name,
                Licensed = m.Licensed,
                Years = m.Years,
                ReviewCount = m.ReviewsCount,
                Rating = m.Rating,
                MediaFileDataId = m.MediaFileDataId,
                ShowReviews = m.ShowReviews
            }).ToList();

            return Json(new 
            {
                count = count,
                careManagers = careManagers
            });
        }

        /// <summary>
        /// Visitor gets CareHomes by coordinates for MapSearch.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCareHomesByCoordinates(double longitude, double latitude)
        {
            var distance = 0.025;
            var minLongitude = longitude - distance;
            var maxLongitude = longitude + distance;
            var minLatitude = latitude - distance;
            var maxLatitude = latitude + distance;

            var homes = db.CareHomes
                .Where(h => h.Longitude > minLongitude && h.Longitude < maxLongitude && h.Latitude > minLatitude && h.Latitude < maxLatitude)
                .Include(h => h.CareManagers)
                .Select(h => new
                {
                    CareHomeId = h.CareHomeId,
                    Code = h.CareHomeCode,
                    Name = h.Name,
                    Latitude = h.Latitude,
                    Longitude = h.Longitude,
                    Address = h.Area.PrefectureName + h.Area.CityName + h.Address + h.AddressBuilding,
                    FileName = h.MediaFileDataId,
                    Rating = h.Rating,
                    CareManagerCount = h.CareManagers.Count,
                    Registered = !string.IsNullOrEmpty(h.UserId)
                })
                .ToList();

            return Json(homes);
        }

        public ActionResult QRCode()
        {
            return View();
        }
    }
}