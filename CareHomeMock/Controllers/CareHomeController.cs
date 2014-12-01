﻿using System;
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
            ViewBag.AreaId = new SelectList(db.Areas, "AreaId", "PrefectureName");
            return View();
        }

        // POST: /CareHome/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CareHomeId,AreaId,Zip,Address,Tel,Fax,WebsiteUrl,Established,CompanyType,CompanyName,ChiefName,ChiefJobTitle,Longitude,Latitude,DataUpdated,介護支援専門員在席人数,介護支援専門員常勤換算,事務員在席人数,事務員常勤換算,その他在席人数,その他常勤換算,全職員在席人数,全職員常勤換算,経験5年以上割合,要介護5,要介護4,要介護3,要介護2,要介護1,要支援2,要支援1,自立,利用者の権利擁護,サービスの質の確保,相談苦情等への対応,外部機関等との連携,事業運営管理,安全衛生管理等,従業者の研修等,MediaFileDataId,Region,Traits,Messages")] CareHome carehome)
        {
            if (ModelState.IsValid)
            {
                db.CareHomes.Add(carehome);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId = new SelectList(db.Areas, "AreaId", "PrefectureName", carehome.AreaId);
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
            ViewBag.AreaId = new SelectList(db.Areas, "AreaId", "PrefectureName", carehome.AreaId);
            return View(carehome);
        }

        // POST: /CareHome/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CareHomeId,AreaId,Zip,Address,Tel,Fax,WebsiteUrl,Established,CompanyType,CompanyName,ChiefName,ChiefJobTitle,Longitude,Latitude,DataUpdated,介護支援専門員在席人数,介護支援専門員常勤換算,事務員在席人数,事務員常勤換算,その他在席人数,その他常勤換算,全職員在席人数,全職員常勤換算,経験5年以上割合,要介護5,要介護4,要介護3,要介護2,要介護1,要支援2,要支援1,自立,利用者の権利擁護,サービスの質の確保,相談苦情等への対応,外部機関等との連携,事業運営管理,安全衛生管理等,従業者の研修等,MediaFileDataId,Region,Traits,Messages")] CareHome carehome)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carehome).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.Areas, "AreaId", "PrefectureName", carehome.AreaId);
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
