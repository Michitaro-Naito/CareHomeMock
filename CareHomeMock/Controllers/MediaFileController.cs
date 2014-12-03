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
    public class MediaFileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MediaFile/
        public ActionResult Index()
        {
            var mediafiles = db.MediaFiles.Include(m => m.CareHome);
            return View(mediafiles.ToList());
        }

        // GET: /MediaFile/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaFile mediafile = db.MediaFiles.Find(id);
            if (mediafile == null)
            {
                return HttpNotFound();
            }
            return View(mediafile);
        }

        // GET: /MediaFile/Create
        public ActionResult Create()
        {
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "CareHomeCode");
            return View();
        }

        // POST: /MediaFile/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MediaFileId,Created,Updated,CareHomeId,Order,Type,RowKey")] MediaFile mediafile)
        {
            if (ModelState.IsValid)
            {
                db.MediaFiles.Add(mediafile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "CareHomeCode", mediafile.CareHomeId);
            return View(mediafile);
        }

        // GET: /MediaFile/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaFile mediafile = db.MediaFiles.Find(id);
            if (mediafile == null)
            {
                return HttpNotFound();
            }
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "CareHomeCode", mediafile.CareHomeId);
            return View(mediafile);
        }

        // POST: /MediaFile/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MediaFileId,Created,Updated,CareHomeId,Order,Type,RowKey")] MediaFile mediafile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mediafile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CareHomeId = new SelectList(db.CareHomes, "CareHomeId", "CareHomeCode", mediafile.CareHomeId);
            return View(mediafile);
        }

        // GET: /MediaFile/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaFile mediafile = db.MediaFiles.Find(id);
            if (mediafile == null)
            {
                return HttpNotFound();
            }
            return View(mediafile);
        }

        // POST: /MediaFile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MediaFile mediafile = db.MediaFiles.Find(id);
            db.MediaFiles.Remove(mediafile);
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
