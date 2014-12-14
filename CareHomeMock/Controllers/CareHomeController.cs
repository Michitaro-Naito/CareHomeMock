using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Models;
using System.Diagnostics;
using CsvHelper;
using CareHomeMock.Helper;

namespace CareHomeMock.Controllers
{
    public class CareHomeController : BaseController
    {
        /// <summary>
        /// Admin views CareHomes.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var carehomes = db.CareHomes.Include(c => c.Area);
            return View(carehomes.Take(50).ToList());
        }

        /// <summary>
        /// Admin adds/edits CareHome here.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            var carehome = db.CareHomes.Find(id) ?? new CareHome();
            ViewBag.AreaId = new SelectList(db.Areas.Select(a => new { AreaId = a.AreaId, CityName = a.PrefectureName + a.CityName }), "AreaId", "CityName", carehome.AreaId);
            return View(carehome);
        }

        /// <summary>
        /// Add/Edit confirmed.
        /// </summary>
        /// <param name="carehome"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CareHome carehome)
        {
            if (ModelState.IsValid)
            {
                if (carehome.CareHomeId == 0)
                {
                    // Add
                    db.CareHomes.Add(carehome);
                }
                else
                {
                    // Edit
                    db.Entry(carehome).State = EntityState.Modified;
                    db.Entry(carehome).Property(h => h.UserId).IsModified = false;
                    //db.Entry(carehome).Property(h => h.CareManagers).IsModified = false;
                }
                db.SaveChanges();
                Log(LogType.Admin, "事業所情報を更新しました。");
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.Areas.Select(a => new { AreaId = a.AreaId, CityName = a.PrefectureName + a.CityName }), "AreaId", "CityName", carehome.AreaId);
            return View(carehome);
        }

        /// <summary>
        /// Admin deactivates CareHome.
        /// Deactivated CareHome is invisible at frontend.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int? id)
        {
            var home = db.CareHomes.Find(id);
            if (home == null)
                return HttpNotFound();
            home.Deactivated = true;
            db.SaveChanges();
            Log(LogType.Admin, "事業所情報を無効化しました。");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Admin activates CareHome.
        /// Activated CareHome is visible at frontend.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int? id)
        {
            var home = db.CareHomes.Find(id);
            if (home == null)
                return HttpNotFound();
            home.Deactivated = false;
            db.SaveChanges();
            Log(LogType.Admin, "事業所情報を有効化しました。");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Admin downloads CareHomes as CSV here.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DownloadCareHomes()
        {
            using (var writer = new System.IO.StreamWriter(Response.OutputStream))
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                foreach (var c in db.CareHomes)
                {
                    csv.WriteField(c.CareHomeId.ToString());
                    csv.WriteField(c.CompanyName);
                    csv.WriteField(c.ChiefName);
                    csv.WriteField(c.Email);
                    csv.NextRecord();
                }
            }
            //Response.ContentType = "text/csv";
            Log(LogType.Admin, "CSVで事業所一覧をダウンロードしました。");
            return null;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DownloadCareManagers()
        {
            using (var writer = new System.IO.StreamWriter(Response.OutputStream))
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                foreach (var m in db.CareManagers)
                {
                    csv.WriteField(m.CareHomeId);
                    csv.WriteField(m.CareManagerId);
                    csv.WriteField(m.Name);
                    csv.WriteField(m.Email);
                    csv.WriteField(m.Birthday);
                    csv.WriteField(m.Gender);
                    csv.WriteField(m.Licensed);
                    //csv.WriteField(m.Licenses);
                    csv.NextRecord();
                }
            }
            //Response.ContentType = "text/csv";
            Log(LogType.Admin, "CSVでケアマネ一覧をダウンロードしました。");
            return null;
        }

        /// <summary>
        /// Admin uploads CSV to update CareHomes.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult UploadCsv()
        {
            return View();
        }

        /// <summary>
        /// CSV is uploaded to update CareHomes.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UploadCsv(HttpPostedFileBase file)
        {
            Debug.WriteLine(file.ContentLength);
            using (var reader = new System.IO.StreamReader(file.InputStream))
            using (var csv = new CsvReader(reader))
            {
                var areas = db.Areas.ToList();
                var line = 0;
                var added = 0;
                var updated = 0;
                while (csv.Read())
                {
                    var row = csv.CurrentRecord;
                    if (row.Length != 39)
                        throw new Exception(string.Format("Fields must be 39. Line:{0} Fields:{1}", line, row.Length));

                    var code = row[0];
                    var home = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
                    if (home == null)
                    {
                        home = new CareHome() { CareHomeCode = code };
                        db.CareHomes.Add(home);
                        added++;
                        Debug.WriteLine("Added: " + code);
                    }
                    else
                    {
                        updated++;
                    }

                    home.Zip = row[1];
                    home.Address = row[2];
                    home.Tel = row[3];
                    home.Fax = row[4];
                    home.WebsiteUrl = row[5];
                    home.Established = DateTime.Parse(row[6]);
                    home.CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), row[7]);
                    home.CompanyName = row[8];
                    home.ChiefName = row[9];
                    home.ChiefJobTitle = row[10];
                    home.Area = areas.First(a => a.CityCode == int.Parse(row[11]));
                    home.Longitude = double.Parse(row[12]);
                    home.Latitude = double.Parse(row[13]);
                    home.DataUpdated = DateTime.Parse(row[14]);
                    home.介護支援専門員在席人数 = double.Parse(row[15]);
                    home.介護支援専門員常勤換算 = double.Parse(row[16]);
                    home.事務員在席人数 = double.Parse(row[17]);
                    home.事務員常勤換算 = double.Parse(row[18]);
                    home.その他在席人数 = double.Parse(row[19]);
                    home.その他常勤換算 = double.Parse(row[20]);
                    home.全職員在席人数 = double.Parse(row[21]);
                    home.全職員常勤換算 = double.Parse(row[22]);
                    home.経験5年以上割合 = double.Parse(row[23]);
                    home.要介護5 = double.Parse(row[24]);
                    home.要介護4 = double.Parse(row[25]);
                    home.要介護3 = double.Parse(row[26]);
                    home.要介護2 = double.Parse(row[27]);
                    home.要介護1 = double.Parse(row[28]);
                    home.要支援2 = double.Parse(row[29]);
                    home.要支援1 = double.Parse(row[30]);
                    home.自立 = double.Parse(row[31]);
                    home.利用者の権利擁護 = double.Parse(row[32]);
                    home.サービスの質の確保 = double.Parse(row[33]);
                    home.相談苦情等への対応 = double.Parse(row[34]);
                    home.外部機関等との連携 = double.Parse(row[35]);
                    home.事業運営管理 = double.Parse(row[36]);
                    home.安全衛生管理等 = double.Parse(row[37]);
                    home.従業者の研修等 = double.Parse(row[38]);

                    line++;
                }
                db.SaveChanges();
                Debug.WriteLine("Lines:{0} Added:{1} Updated:{2}", line, added, updated);
            }
            db.SaveChanges();
            Log(LogType.Admin, "CSVで事業所一覧を更新しました。");
            return null;
        }

        /// <summary>
        /// CareHomeUser edits it's additional info.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditAdditionalInfo(string code)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var careHome = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (careHome == null)
                return HttpNotFound();

            return View(careHome);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdditionalInfo(CareHome careHome, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 200000)
                ModelState.AddModelError("", "アップロードできる画像のサイズは200kBまでです。");

            ModelState.Remove("CareHomeCode");

            if (ModelState.IsValid)
            {
                var home = db.CareHomes.Find(careHome.CareHomeId);
                home.Region = careHome.Region;
                home.Traits = careHome.Traits;
                home.Messages = careHome.Messages;

                if (file != null)
                {
                    BlobHelper.DeleteIfExists("mediafile", home.MediaFileDataId);
                    home.MediaFileDataId = BlobHelper.Upload("mediafile", file, file.FileName);
                }

                db.SaveChanges();

                Log(LogType.CareHome, "追加情報を更新しました。");
                Flash("保存しました。");
                return RedirectToAction("CareHomeMenu", "Home");
            }
            return View(careHome);
        }
    }
}
