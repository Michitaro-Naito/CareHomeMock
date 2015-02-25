using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CareHomeMock.Controllers
{
    /// <summary>
    /// Admin accepts / denies applications here.
    /// </summary>
    public class ApplicationController : BaseController
    {
        /// <summary>
        /// Visitor sends an application to Admin.
        /// He receives ID and password if accepted.
        /// </summary>
        /// <returns></returns>
        public ActionResult Send(string careHomeCode)
        {
            if (string.IsNullOrEmpty(careHomeCode))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var home = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == careHomeCode);
            if (home == null)
                return HttpNotFound();
            return View(new ApplicationSendVM() { CareHomeCode = home.CareHomeCode, CareHomeName = home.CompanyName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(ApplicationSendVM model)
        {
            if (model.CareHomeCode == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var home = db.CareHomes.FirstOrDefault(h => h.CareHomeCode == model.CareHomeCode);
            if (home == null)
                return HttpNotFound();
            if (home.User != null)
                ModelState.AddModelError("CareHomeCode", "この事業所会員はすでに登録されています。");

            if (ModelState.IsValid)
            {
                // Storeas an application.
                var application = new Application()
                {
                    CareHome = home,
                    IpAddress = Request.UserHostAddress,
                    Email = model.EmailPersonInCharge,
                    Name = model.NamePersonInCharge,
                    Note = model.Note
                };
                db.Applications.Add(application);
                db.SaveChanges();
                SendEmailToAdmin("[ケアマネ情報局] 事業所会員登録申請がありました", "先ほど申請を受理いたしました。管理画面をご確認ください。");
                Flash("事業所会員の登録申請を送信しました。事務局でご本人確認をさせていただき、承認された場合は管理者用のIDとパスワードをご連絡いたします。");
                Log(LogType.Others, "事業所会員登録を申請しました。");
                //return RedirectToAction("CareHomeInfo_BasicInfo", "Home", new { code = home.CareHomeCode });
                return RedirectToAction("Details", "StaticPage", new { id = 8 });
            }
            return View(model);
        }

        /// <summary>
        /// Admin views Applications.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.CareHome);
            return View(applications.ToList());
        }

        /// <summary>
        /// Admin confirms to approve.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var application = db.Applications.Find(id);
            if (application == null)
                return HttpNotFound();
            return View(application);
        }

        /// <summary>
        /// Confirmed. Approve and notify.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveConfirmed(int id, string noteForSender)
        {
            var application = db.Applications.Find(id);
            if (application == null)
                return HttpNotFound();

            if (application.CareHome.User != null)
                throw new InvalidOperationException("該当する事業所会員は既に登録されています。");

            // Generates username and password.
            var username = "KT" + application.CareHome.CareHomeCode;
            var password = System.Web.Security.Membership.GeneratePassword(12, 1);

            // Registers CareHomeUser.
            var user = new User() { UserName = username, Email = application.Email, Name = application.Name };
            var result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("会員を登録できませんでした。");

            var registeredUser = db.Users.FirstOrDefault(u => u.UserName == username);
            if (registeredUser == null)
                throw new Exception("登録されたはずの会員が見つかりませんでした。");

            // Notifies Sender.
            //SendEmail(registeredUser.Email, "[ケアマネ情報局] 事業所会員として承認されました", string.Format("ID:{0} password:{1} 備考:{2}", username, password, noteForSender));
            dynamic email = new Postal.Email("ApplicationApproved");
            email.To = registeredUser.Email;
            email.CareHomeName = application.CareHome.Name;
            email.CareHomeUserName = application.Name;
            email.UserId = username;
            email.Password = password;
            email.SiteUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
            email.Note = noteForSender;
            email.Send();

            // Adds CareHome and deletes Application.
            registeredUser.CareHomes.Add(application.CareHome);
            db.Applications.Remove(application);
            db.SaveChanges();

            Flash(string.Format("承認されました。ID:{0}", username));
            Log(LogType.Admin, "事業所会員登録申請を承認しました。");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Admin confirms to reject.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Reject(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var application = db.Applications.Find(id);
            if (application == null)
                return HttpNotFound();
            return View(application);
        }

        /// <summary>
        /// Confirmed. Reject and notify.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public ActionResult RejectConfirmed(int id, string noteForSender)
        {
            var application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();

            // Notifies Sender.
            SendEmail(application.Email, "[ケアマネ情報局] 事業所会員として承認されませんでした", string.Format("備考:{0}", noteForSender));

            Flash("拒否されました。");
            Log(LogType.Admin, "事業所会員登録申請を拒否しました。");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Admin confirms just to delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        /// <summary>
        /// Confirmed. Just delete and notify nobody.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            Flash("削除されました。");
            Log(LogType.Admin, "事業所会員登録申請を削除しました。");
            return RedirectToAction("Index");
        }
    }
}
