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
using System.IO;
using System.Text.RegularExpressions;

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
        /// Admin uploads a CSV to blob.
        /// It doesn't update CareHome data at this time.
        /// When CareHome.UpdateCareHomesFromCsv is called, it will be updated.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UploadCsv(HttpPostedFileBase file)
        {
            if (file == null)
            {
                Flash("アップロードするCSVを選択してください。");
            }
            else
            {
                var container = BlobHelper.GetContainer("csv");
                var blob = container.GetBlockBlobReference("CareHome.UploadCsv");
                blob.UploadFromStream(file.InputStream);
                Flash("CSVをアップロードし、更新の準備ができました。更新ボタンを押して更新処理を開始してください。");
            }

            return View();

            /*Debug.WriteLine(file.ContentLength);
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
            return null;*/
        }

        /// <summary>
        /// Updates CareHomes from CSV.
        /// CSV is uploaded to Blob at CareHome.UploadCsv.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult UpdateFromCsv(int skip = 0, int take = 500)
        {
            var container = BlobHelper.GetContainer("csv");
            var blob = container.GetBlockBlobReference("CareHome.UploadCsv");
            using (var mem = new MemoryStream())
            {
                blob.DownloadToStream(mem);
                mem.Seek(0, SeekOrigin.Begin);

                using (var reader = new System.IO.StreamReader(mem, System.Text.Encoding.GetEncoding("Shift_JIS")))
                using (var csv = new CsvReader(reader))
                {
                    for (var n = 0; n < skip; n++)
                        // Skips until the first line.
                        csv.Read();

                    var japaneseCalendarRegex = new Regex(@"^(?<jpyear>\d{4})-(?<month>\d{2})-(?<day>\d{2})$");
                    var added = 0;
                    var updated = 0;
                    while ((added + updated) < take && csv.Read())
                    {
                        string code = null;
                        try
                        {
                            var row = csv.CurrentRecord;
                            if (row.Length != 44)
                                return Json(new { success = false, error = string.Format("列の数が正しくありません。行:{0} 必要な列数:{1} 実際の列数:{2}", skip + added + updated, 44, row.Length), added = added, updated = updated });

                            Debug.WriteLine(row[0] + row[1]);

                            code = row[0];  // 事業所ID
                            var home = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
                            if (home == null)
                            {
                                home = new CareHome() { CareHomeCode = code };
                                db.CareHomes.Add(home);
                                added++;
                            }
                            else
                            {
                                updated++;
                            }

                            home.Name = csv.GetField<string>("事業所名");
                            home.Zip = csv.GetField<string>("郵便番号");
                            // 都道府県 => city
                            // 市区町村 => city
                            home.Address = csv.GetField<string>("番地");
                            home.AddressBuilding = csv.GetField<string>("建物名部屋番号等");
                            var cityCode = csv.GetField<int>("city");
                            home.Area = db.Areas.First(a => a.CityCode == cityCode);
                            home.Longitude = csv.GetField<double>("fX");
                            home.Latitude = csv.GetField<double>("fY");
                            home.Tel = csv.GetField<string>("Tel");
                            home.Fax = csv.GetField<string>("Fax");
                            home.WebsiteUrl = csv.GetField<string>("ホームページ");
                            var match = japaneseCalendarRegex.Match(csv.GetField<string>("事業開始日"));
                            if (match.Success)
                            {
                                // Japanese Calendar format like "0025-08-26".
                                var year = 1988 + int.Parse(match.Groups["jpyear"].ToString());
                                var month = int.Parse(match.Groups["month"].ToString());
                                var day = int.Parse(match.Groups["day"].ToString());
                                if (year == 1988 && month == 0 && day == 0)
                                    home.Established = null;
                                else
                                    home.Established = new DateTime(year, month, day);
                            }
                            else
                            {
                                home.Established = csv.GetField<DateTime>("事業開始日");
                            }
                            home.CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), csv.GetField<string>("法人区分").Replace('・', '_').Replace('（', '_').Replace('）', '_'));
                            home.CompanyName = csv.GetField<string>("法人名称");
                            home.ChiefName = csv.GetField<string>("施設管理者氏名");
                            home.ChiefJobTitle = csv.GetField<string>("施設管理者職名");
                            match = japaneseCalendarRegex.Match(csv.GetField<string>("情報更新日"));
                            if (match.Success)
                            {
                                // Japanese Calendar format like "0025-08-26".
                                var year = 1988 + int.Parse(match.Groups["jpyear"].ToString());
                                var month = int.Parse(match.Groups["month"].ToString());
                                var day = int.Parse(match.Groups["day"].ToString());
                                if (year == 1988 && month == 0 && day == 0)
                                    home.DataUpdated = null;
                                else
                                    home.DataUpdated = new DateTime(year, month, day);
                            }
                            else
                            {
                                home.DataUpdated = csv.GetField<DateTime>("情報更新日");
                            }
                            home.介護支援専門員在席人数 = csv.GetField<Nullable<double>>("介護支援専門員_合計");
                            home.介護支援専門員常勤換算 = csv.GetField<Nullable<double>>("介護支援専門員_常勤換算人数");
                            home.事務員在席人数 = csv.GetField<Nullable<double>>("事務員_合計");
                            home.事務員常勤換算 = csv.GetField<Nullable<double>>("事務員_常勤換算人数");
                            home.その他在席人数 = csv.GetField<Nullable<double>>("その他_合計");
                            home.その他常勤換算 = csv.GetField<Nullable<double>>("その他_常勤換算人数");
                            home.全職員在席人数 = csv.GetField<Nullable<double>>("全職員_合計");
                            home.全職員常勤換算 = csv.GetField<Nullable<double>>("全職員_常勤換算人数");
                            home.経験5年以上割合 = csv.GetField<Nullable<double>>("経験5年以上割合");
                            home.利用者数 = csv.GetField<Nullable<double>>("利用者数");
                            home.自立 = csv.GetField<Nullable<double>>("自立");
                            home.要介護5 = csv.GetField<Nullable<double>>("要介護５");
                            home.要介護4 = csv.GetField<Nullable<double>>("要介護４");
                            home.要介護3 = csv.GetField<Nullable<double>>("要介護３");
                            home.要介護2 = csv.GetField<Nullable<double>>("要介護２");
                            home.要介護1 = csv.GetField<Nullable<double>>("要介護１");
                            home.要支援2 = csv.GetField<Nullable<double>>("要支援２");
                            home.要支援1 = csv.GetField<Nullable<double>>("要支援１");
                            home.利用者の権利擁護 = csv.GetField<Nullable<double>>("利用者の権利擁護");
                            home.サービスの質の確保 = csv.GetField<Nullable<double>>("サービスの質の確保");
                            home.相談苦情等への対応 = csv.GetField<Nullable<double>>("相談・苦情等への対応");
                            home.外部機関等との連携 = csv.GetField<Nullable<double>>("外部機関等との連携");
                            home.事業運営管理 = csv.GetField<Nullable<double>>("事業運営・管理");
                            home.安全衛生管理等 = csv.GetField<Nullable<double>>("安全・衛生管理等");
                            home.従業者の研修等 = csv.GetField<Nullable<double>>("従業者の研修等");

                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            return Json(new { success = false, error = (skip + added + updated - 1) + "行目" + code + "の更新中にエラーが発生しました。" + e.ToString(), added = added, updated = updated });
                        }
                    }

                    return Json(new { success = true, error = "", added = added, updated = updated });
                }
            }
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
