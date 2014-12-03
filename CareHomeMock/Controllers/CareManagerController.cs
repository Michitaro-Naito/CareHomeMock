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
        public ActionResult Index(int? careHomeId)
        {
            if (careHomeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var home = db.CareHomes.FirstOrDefault(h => h.CareHomeId == careHomeId);
            if (home == null)
                return HttpNotFound();

            var careManagers = home.CareManagers.ToList();

            //var caremanagers = db.CareManagers.Where(m=>m.CareHomeId == careHomeId).Include(c => c.CareHome);

            return View(new CareManagerIndexVM() {
                CareHomeId = careHomeId.Value,
                CareManagers = careManagers
            });
        }

        // GET: /CareManager/Create
        public ActionResult Create(int? careHomeId)
        {
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip");
            return View();
        }

        // POST: /CareManager/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CareManagerId,CareHomeId,MediaFileDataId,Name,Gender,Age,Licensed,Licenses,CurrentPatients,AllowNewPatient,Career,Messages,BlogUrls,TotalRating,ReviewsCount,Rating,Birthday")] CareManager caremanager)
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
        public ActionResult Edit(int?careHomeId, int? careManagerId)
        {
            if (careHomeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var careManager = new CareManager();
            if(careManagerId != null)
            {
                // Edit
                careManager = db.CareManagers.FirstOrDefault(m => m.CareHomeId == careHomeId && m.CareManagerId == careManagerId);
                if (careManager == null)
                    return HttpNotFound();
            }

            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "Zip", careManager.CareHomeId);
            return View(careManager);
        }

        // POST: /CareManager/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CareManagerId,CareHomeId,MediaFileDataId,Name,Gender,Age,Licensed,Licenses,CurrentPatients,AllowNewPatient,Career,Messages,BlogUrls,TotalRating,ReviewsCount,Rating,Birthday")] CareManager caremanager)
        {
            if (ModelState.IsValid)
            {
                if (caremanager.CareManagerId == 0)
                {
                    // Add
                    db.CareManagers.Add(caremanager);
                }
                else
                {
                    // Edit
                    db.Entry(caremanager).State = EntityState.Modified;
                }
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
