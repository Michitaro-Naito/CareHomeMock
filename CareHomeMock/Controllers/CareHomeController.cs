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
    /// Admin controls CareHomes here.
    /// </summary>
    public class CareHomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /CareHome/
        public ActionResult Index()
        {
            var carehomes = db.CareHomes.Include(c => c.Area);
            return View(carehomes.ToList());
        }

        // GET: /CareHome/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareHome carehome = db.CareHomes.Find(id);
            if (carehome == null)
            {
                return HttpNotFound();
            }
            return View(carehome);
        }

        // GET: /CareHome/Create
        public ActionResult Create()
        {
            ViewBag.CityCode = new SelectList(db.Areas, "CityCode", "PrefectureName");
            return View();
        }

        // POST: /CareHome/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CareHomeId,CityCode,Zip,Address,Tel,Fax,WebsiteUrl,Established,CompanyType,CompanyName,ChiefName,ChiefJobTitle,Longitude,Latitude,DataUpdated,MediaFileDataId,Region,Traits,Messages")] CareHome carehome)
        {
            if (ModelState.IsValid)
            {
                db.CareHomes.Add(carehome);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityCode = new SelectList(db.Areas, "CityCode", "PrefectureName", carehome.CityCode);
            return View(carehome);
        }

        // GET: /CareHome/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareHome carehome = db.CareHomes.Find(id);
            if (carehome == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityCode = new SelectList(db.Areas, "CityCode", "PrefectureName", carehome.CityCode);
            return View(carehome);
        }

        // POST: /CareHome/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CareHomeId,CityCode,Zip,Address,Tel,Fax,WebsiteUrl,Established,CompanyType,CompanyName,ChiefName,ChiefJobTitle,Longitude,Latitude,DataUpdated,MediaFileDataId,Region,Traits,Messages")] CareHome carehome)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carehome).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityCode = new SelectList(db.Areas, "CityCode", "PrefectureName", carehome.CityCode);
            return View(carehome);
        }

        // GET: /CareHome/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareHome carehome = db.CareHomes.Find(id);
            if (carehome == null)
            {
                return HttpNotFound();
            }
            return View(carehome);
        }

        // POST: /CareHome/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CareHome carehome = db.CareHomes.Find(id);
            db.CareHomes.Remove(carehome);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UploadCsv()
        {
            return View();
        }

        public ActionResult DownloadCsvCareHomes()
        {
            return View();
        }

        public ActionResult DownloadCsvCareManagers()
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
