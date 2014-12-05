using CareHomeMock.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class ToolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Tool/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportArea()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportArea(HttpPostedFileBase file)
        {
            Debug.WriteLine(file.ContentLength);
            using (var reader = new System.IO.StreamReader(file.InputStream))
            using (var csv = new CsvReader(reader))
            {
                var line = 0;
                while (csv.Read())
                {
                    var row = csv.CurrentRecord;
                    if (row.Length != 7)
                        throw new Exception(string.Format("Fields must be 7. Line:{0} Fields:{1}", line, row.Length));
                    db.Areas.Add(new Area()
                    {
                        PrefectureCode = int.Parse(row[0]),
                        PrefectureName = row[1],
                        CityCode = int.Parse(row[2]),
                        CityName = row[3],
                        ZoneCode = int.Parse(row[4]),
                        ZoneName = row[5],
                        ZonePopulation = int.Parse(row[6])
                    });
                    line++;
                }
                db.SaveChanges();
                Debug.WriteLine("{0} lines imported.", line);
            }
            db.SaveChanges();
            return null;
        }

        public ActionResult PopulateDummyData()
        {
            var start = DateTime.UtcNow;

            var areas = db.Areas.ToList();
            var rand = new Random();

            for (var n = 0; n < 1000; n++)
            {
                var home = new CareHome();
                home.Address = string.Format("{0}番地", n);
                home.Area = areas[rand.Next(areas.Count)];
                home.CareHomeCode = (10000 + n).ToString();
                home.ChiefJobTitle = string.Format("施設長{0}", n);
                home.ChiefName = string.Format("責任一郎{0}", n);
                home.CompanyName = string.Format("○○ケア{0}", n);
                home.CompanyType = new[]{ CompanyType.営利法人, CompanyType.非営利法人 }[rand.Next(2)];
                home.DataUpdated = DateTime.UtcNow.AddDays(-rand.Next(365));
                home.Email = string.Format("ichiro{0}@example.com", n);
                home.Established = DateTime.UtcNow.AddYears(-rand.Next(20));
                home.Fax = string.Format("{0:D3}-{1:D4}-{2:D4}", rand.Next(1000), rand.Next(10000), rand.Next(10000));
                home.Latitude = 33.0 + 4.0 * rand.NextDouble();
                home.Longitude = 133.0 + 4.0 * rand.NextDouble();
                home.Messages = string.Format("事業所が入力したメッセージです。{0}", n);
                home.Rating = 5.0 * rand.NextDouble();
                home.Region = string.Format("事業所が入力した対応可能地域です。{0}", n);
                home.ReviewCount = rand.Next(1000);
                home.Tel = string.Format("{0:D3}-{1:D4}-{2:D4}", rand.Next(1000), rand.Next(10000), rand.Next(10000));
                home.Traits = string.Format("事業所が入力した特徴・セールスポイントです。{0}", n);
                home.WebsiteUrl = string.Format("http://example.com/{0}/", n);
                home.Zip = string.Format("{0:D3}-{1:D4}", rand.Next(1000), rand.Next(10000));
                home.サービスの質の確保 = 5.0 * rand.NextDouble();
                home.その他在席人数 = rand.Next(100);
                home.その他常勤換算 = 100.0 * rand.NextDouble();
                home.安全衛生管理等 = 5.0 * rand.NextDouble();
                home.介護支援専門員在席人数 = rand.Next(100);
                home.介護支援専門員常勤換算 = 100.0 * rand.NextDouble();
                home.外部機関等との連携 = 5.0 * rand.NextDouble();
                home.経験5年以上割合 = rand.NextDouble();
                home.事業運営管理 = 5.0 * rand.NextDouble();
                home.事務員在席人数 = rand.Next(100);
                home.事務員常勤換算 = 100.0 * rand.NextDouble();
                home.自立 = rand.Next(100);
                home.従業者の研修等 = 5.0 * rand.NextDouble();
                home.全職員在席人数 = home.介護支援専門員在席人数 + home.事務員在席人数 + home.その他在席人数;
                home.全職員常勤換算 = home.介護支援専門員常勤換算 + home.事務員常勤換算 + home.その他常勤換算;
                home.相談苦情等への対応 = 5.0 * rand.NextDouble();
                home.要介護1 = rand.Next(100);
                home.要介護2 = rand.Next(100);
                home.要介護3 = rand.Next(100);
                home.要介護4 = rand.Next(100);
                home.要介護5 = rand.Next(100);
                home.要支援1 = rand.Next(100);
                home.要支援2 = rand.Next(100);
                home.利用者の権利擁護 = 5.0 * rand.NextDouble();

                db.CareHomes.Add(home);
            }
            db.SaveChanges();
            Response.Write(string.Format("Populated. {0}ms", (DateTime.UtcNow - start).TotalMilliseconds));
            return null;
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