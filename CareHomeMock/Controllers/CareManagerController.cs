using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Models;
using CareHomeMock.Helper;
using System.Diagnostics;

namespace CareHomeMock.Controllers
{
    public class CareManagerController : BaseController
    {
        /// <summary>
        /// CareHome manages it's CareManagers here.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(string code)
        {
            if (CurrentUser == null)
                throw new Exception("会員情報が見つかりません。");

            CareHome home = null;
            if (code == null)
            {
                home = CurrentUser.CareHomes.FirstOrDefault();
                if (home == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index", new { code = home.CareHomeCode });
            }

            home = CurrentUser.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
            if (home == null)
                return HttpNotFound();

            var careManagers = home.CareManagers.ToList();

            return View(new CareManagerIndexVM() {
                CareHomeId = home.CareHomeId,
                CareHomeCode = home.CareHomeCode,
                CareManagers = careManagers
            });
        }

        /// <summary>
        /// CareHome adds/edits it's CareManager info.
        /// </summary>
        /// <param name="careHomeId"></param>
        /// <param name="careManagerId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(string code, int? careManagerId)
        {
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (careManagerId != null && !CurrentUser.CareHomes.Any(h => h.CareManagers.Any(c => c.CareManagerId == careManagerId)))
                // Not owned
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var careManager = new CareManager();
            if (careManagerId == null)
            {
                // Add
                var home = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == code);
                if (home == null)
                    return HttpNotFound();
                careManager.CareHomeId = home.CareHomeId;
            }
            else
            {
                // Edit
                careManager = db.CareManagers.FirstOrDefault(m => m.CareHome.CareHomeCode == code && m.CareManagerId == careManagerId);
                if (careManager == null)
                    return HttpNotFound();
            }

            //ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", careManager.CareHomeId);
            ViewBag.Gender = EnumHelper<Gender>.GetSelectList(careManager.Gender);
            return View(careManager);
        }

        // POST: /CareManager/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string code, [Bind(Include = "CareManagerId,CareHomeId,Email,MediaFileDataId,Name,Gender,Age,Licensed,Licenses,CurrentPatients,AllowNewPatient,Career,Messages,BlogUrls,TotalRating,ReviewsCount,Rating,Birthday")] CareManager caremanager, HttpPostedFileBase file)
        {
            // Checks file size.
            if (file != null && file.ContentLength > 200000)
                ModelState.AddModelError("", "アップロードできる画像のサイズは200kBまでです。");

            if (ModelState.IsValid)
            {
                // Uploads Image
                if (file != null)
                {
                    BlobHelper.DeleteIfExists("mediafile", caremanager.MediaFileDataId);
                    caremanager.MediaFileDataId = BlobHelper.Upload("mediafile", file, file.FileName);
                }

                // Adds / Edits record
                if (caremanager.CareManagerId == 0)
                {
                    // Add
                    db.CareManagers.Add(caremanager);
                    var verification = new EmailVerification() { CareManager = caremanager, Email = caremanager.Email };
                    db.EmailVerifications.Add(verification);
                    db.SaveChanges();

                    // Notifies CareManager to verify.
                    //SendEmail(caremanager.Email, "[ケアマネ情報局] ケアマネ会員認証", "URL: " + Url.Action("Verify", "EmailVerification", new { verificationCode = verification.VerificationCode}, Request.Url.Scheme));
                    var added = db.CareManagers.Include(m => m.CareHome).FirstOrDefault(m => m.CareManagerId == caremanager.CareManagerId);
                    dynamic email = new Postal.Email("CareManagerAdded");
                    email.To = added.Email;
                    email.CareManagerName = added.Name;
                    email.CareHomeName = added.CareHome.Name;
                    email.CareHomeUserName = "";
                    if (added.CareHome.User != null)
                        email.CareHomeUserName = added.CareHome.User.Name;
                    email.VerificationUrl = Url.Action("Verify", "EmailVerification", new { verificationCode = verification.VerificationCode }, Request.Url.Scheme);
                    email.SiteUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                    email.Send();
                }
                else
                {
                    // Edit
                    var entry = db.Entry(caremanager);
                    entry.State = EntityState.Unchanged;
                    entry.Property(p => p.Email).IsModified = true;
                    entry.Property(p => p.Name).IsModified = true;
                    entry.Property(p => p.Gender).IsModified = true;
                    entry.Property(p => p.Licensed).IsModified = true;
                    entry.Property(p => p.Birthday).IsModified = true;
                    if(file != null)
                        entry.Property(p => p.MediaFileDataId).IsModified = true;
                    db.SaveChanges();
                }
                Log(LogType.CareHome, "所属するケアマネの情報を更新しました。");
                return RedirectToAction("Index");
            }
            //ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", caremanager.CareHomeId);
            ViewBag.Gender = EnumHelper<Gender>.GetSelectList(caremanager.Gender);
            return View(caremanager);
        }

        // GET: /CareManager/Delete/5
        public ActionResult Delete(int? careManagerId)
        {
            if (careManagerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareManager caremanager = db.CareManagers.Find(careManagerId);
            if (caremanager == null)
            {
                return HttpNotFound();
            }
            return View(caremanager);
        }

        // POST: /CareManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int careManagerId)
        {
            CareManager caremanager = db.CareManagers.Find(careManagerId);
            db.CareManagers.Remove(caremanager);
            db.SaveChanges();
            Log(LogType.CareHome, "所属するケアマネの情報を削除しました。");
            return RedirectToAction("Index");
        }



        /// <summary>
        /// CareManager edits his additional info.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditAdditionalData()
        {
            var careManager = CurrentUser.CareManager.FirstOrDefault();
            if (careManager == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.Licenses = new MultiSelectList(db.Licenses, "LicenseId", "Name", careManager.CareManagerLicenses.Select(l => l.LicenseId));
            return View(careManager);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdditionalData(CareManager model, HttpPostedFileBase file, List<int> Licenses)
        {
            var allow = "CurrentPatients,AllowNewPatient,Career,Messages,BlogUrls,ShowReviews,企画立案力,行動実践力,関係構築力,指導管理力,公平中立力,医療知識,介護知識";
            
            // Removes fields from ModelState which are not incoming.
            var allowedFields = allow.Split(',');
            var fieldsToRemove = ModelState.Keys
                .Where(key => !allowedFields.Contains(key))
                .ToList();
            fieldsToRemove.ForEach(f => ModelState.Remove(f));

            // Licenses
            if (Licenses == null)
                Licenses = new List<int>();
            model.CareManagerLicenses.Clear();
            foreach (var l in Licenses)
                model.CareManagerLicenses.Add(new CareManagerLicenses() { LicenseId = l });

            if (ModelState.IsValid)
            {
                var careManager = CurrentUser.CareManager.FirstOrDefault();
                if (careManager == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                // Licenses
                db.CareManagerLicenses.RemoveRange(careManager.CareManagerLicenses);
                careManager.CareManagerLicenses = model.CareManagerLicenses;

                // Updates SQL
                model.CopyTo(ref careManager, allow);   // Copies fields only which are incoding.
                db.SaveChanges();

                Log(LogType.CareManager, "追加情報を更新しました。");
                Flash("保存されました。");
            }
            ViewBag.Licenses = new MultiSelectList(db.Licenses, "LicenseId", "Name", model.CareManagerLicenses.Select(l => l.LicenseId));
            return View(model);
        }
    }
}
