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
    public class StaticPageController : BaseController
    {
        /// <summary>
        /// Visitor sees a static page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            var page = db.StaticPage.Find(id);
            if (page == null)
                return HttpNotFound();
            return View(page);
        }

        /// <summary>
        /// Visitor sees an additional CSS.
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration=60)]
        public ActionResult Css()
        {
            var css = db.Csses.FirstOrDefault();
            if (css == null)
                return HttpNotFound();
            Response.ContentType = "text/css";
            Response.Write(css.Body);
            return null;
        }

        // GET: /StaticPage/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.StaticPage.OrderBy(p=>p.Order).ThenByDescending(p=>p.Updated).ToList());
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
                Log(LogType.Admin, "固定ページを更新しました。", new { staticpage.Title });
                return RedirectToAction("Index");
            }
            return View(staticpage);
        }

        /// <summary>
        /// Admin edits an additional CSS.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        public ActionResult EditCss()
        {
            var css = db.Csses.FirstOrDefault()
                ?? new Css();
            return View(css);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult EditCss(Css model)
        {
            var css = db.Csses.FirstOrDefault();
            if (css == null)
            {
                css = new Css();
                db.Csses.Add(css);
            }
            css.Body = model.Body;
            db.SaveChanges();

            return RedirectToAction("Index");
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
            Log(LogType.Admin, "固定ページを削除しました。", new { staticpage.Title });
            return RedirectToAction("Index");
        }
    }
}
