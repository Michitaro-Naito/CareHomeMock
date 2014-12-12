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
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.StaticPage.ToList());
        }

        // GET: /StaticPage/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            StaticPage staticPage;
            if (id == null)
            {
                // Creates a new StaticPage.
                staticPage = new StaticPage();
            }
            else
            {
                staticPage = db.StaticPage.Find(id);
            }
            if (staticPage == null)
                return HttpNotFound();
            return View(staticPage);
        }

        // POST: /StaticPage/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="StaticPageId,Created,Updated,Order,Title,Html")] StaticPage staticpage)
        {
            if (ModelState.IsValid)
            {
                if (staticpage.StaticPageId == 0)
                {
                    staticpage.Created = staticpage.Updated = DateTime.UtcNow;
                    db.StaticPage.Add(staticpage);
                }
                else
                {
                    staticpage.Updated = DateTime.UtcNow;
                    db.Entry(staticpage).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staticpage);
        }

        // GET: /StaticPage/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
