﻿using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class PartialController : BaseController
    {
        public ActionResult MainLinks()
        {
            using (var db = new ApplicationDbContext())
            {
                var pages = db.StaticPage
                    .Where(p => !p.NotListed)
                    .OrderBy(p => p.Order)
                    .ThenByDescending(p => p.Updated)
                    .ToList();

                return View(pages);
            }
        }
	}
}