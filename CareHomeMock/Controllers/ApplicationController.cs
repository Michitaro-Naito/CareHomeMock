using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Models;

namespace CareHomeMock.Controllers
{
    /// <summary>
    /// Admin accepts / denies applications here.
    /// </summary>
    public class ApplicationController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
                Flash("登録申請を送信しました。後ほど管理者がご連絡を差し上げます。");
                return RedirectToAction("CareHomeInfo_BasicInfo", "Home", new { code = home.CareHomeCode });
            }
            return View(model);
        }

        // GET: /Application/
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.CareHome);
            return View(applications.ToList());
        }

        // GET: /Application/Details/5
        public ActionResult Details(int? id)
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

        // GET: /Application/Create
        public ActionResult Create()
        {
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip");
            return View();
        }

        // POST: /Application/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ApplicationId,CareHomeId,Email,Name,Note")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", application.CareHomeId);
            return View(application);
        }

        // GET: /Application/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", application.CareHomeId);
            return View(application);
        }

        // POST: /Application/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ApplicationId,CareHomeId,Email,Name,Note")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", application.CareHomeId);
            return View(application);
        }

        // GET: /Application/Delete/5
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

        // POST: /Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
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
