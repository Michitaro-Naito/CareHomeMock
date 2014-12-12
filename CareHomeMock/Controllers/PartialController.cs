using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class PartialController : Controller
    {
        public ActionResult MainLinks()
        {
            using (var db = new ApplicationDbContext())
            {
                var pages = db.StaticPage
                    .OrderBy(p => p.Order)
                    .ThenByDescending(p => p.Created)
                    .ToList();

                return View(pages);
            }
        }
	}
}