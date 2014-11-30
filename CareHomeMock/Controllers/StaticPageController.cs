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
    /// Admin controls StaticPages here.
    /// </summary>
    public class StaticPageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /StaticPage/
        public ActionResult Index()
        {
            return View(db.StaticPage.ToList());
        }

        // GET: /StaticPage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticPage staticpage = db.StaticPage.Find(id);
            if (staticpage == null)
            {
                return HttpNotFound();
            }
            return View(staticpage);
        }

        // GET: /StaticPage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /StaticPage/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="StaticPageId,Created,Updated,Order,Title,Html")] StaticPage staticpage)
        {
            if (ModelState.IsValid)
            {
                db.StaticPage.Add(staticpage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staticpage);
        }

        // GET: /StaticPage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticPage staticpage = db.StaticPage.Find(id);
            if (staticpage == null)
            {
                return HttpNotFound();
            }
            return View(staticpage);
        }

        // POST: /StaticPage/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="StaticPageId,Created,Updated,Order,Title,Html")] StaticPage staticpage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staticpage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staticpage);
        }

        // GET: /StaticPage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaticPage staticpage = db.StaticPage.Find(id);
            if (staticpage == null)
            {
                return HttpNotFound();
            }
            return View(staticpage);
        }

        // POST: /StaticPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaticPage staticpage = db.StaticPage.Find(id);
            db.StaticPage.Remove(staticpage);
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
