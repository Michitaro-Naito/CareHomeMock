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
    public class CareManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /CareManager/
        public ActionResult Index()
        {
            var caremanagers = db.CareManagers.Include(c => c.CareHome);
            return View(caremanagers.ToList());
        }

        // GET: /CareManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareManager caremanager = db.CareManagers.Find(id);
            if (caremanager == null)
            {
                return HttpNotFound();
            }
            return View(caremanager);
        }

        // GET: /CareManager/Create
        public ActionResult Create()
        {
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip");
            return View();
        }

        // POST: /CareManager/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CareManagerId,CareHomeId,MediaFileDataId,Name,Gender,Age,Licensed,Licenses,CurrentPatients,AllowNewPatient,Career,Messages,BlogUrls,TotalRating,ReviewsCount,Rating")] CareManager caremanager)
        {
            if (ModelState.IsValid)
            {
                db.CareManagers.Add(caremanager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", caremanager.CareHomeId);
            return View(caremanager);
        }

        // GET: /CareManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareManager caremanager = db.CareManagers.Find(id);
            if (caremanager == null)
            {
                return HttpNotFound();
            }
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", caremanager.CareHomeId);
            return View(caremanager);
        }

        // POST: /CareManager/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CareManagerId,CareHomeId,MediaFileDataId,Name,Gender,Age,Licensed,Licenses,CurrentPatients,AllowNewPatient,Career,Messages,BlogUrls,TotalRating,ReviewsCount,Rating")] CareManager caremanager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caremanager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", caremanager.CareHomeId);
            return View(caremanager);
        }

        // GET: /CareManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CareManager caremanager = db.CareManagers.Find(id);
            if (caremanager == null)
            {
                return HttpNotFound();
            }
            return View(caremanager);
        }

        // POST: /CareManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CareManager caremanager = db.CareManagers.Find(id);
            db.CareManagers.Remove(caremanager);
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
