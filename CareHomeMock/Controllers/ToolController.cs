using CareHomeMock.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class ToolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Tool/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportArea()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportArea(HttpPostedFileBase file)
        {
            Debug.WriteLine(file.ContentLength);
            using (var reader = new System.IO.StreamReader(file.InputStream))
            using (var csv = new CsvReader(reader))
            {
                var line = 0;
                while (csv.Read())
                {
                    var row = csv.CurrentRecord;
                    if (row.Length != 7)
                        throw new Exception(string.Format("Fields must be 7. Line:{0} Fields:{1}", line, row.Length));
                    db.Areas.Add(new Area()
                    {
                        PrefectureCode = int.Parse(row[0]),
                        PrefectureName = row[1],
                        CityCode = int.Parse(row[2]),
                        CityName = row[3],
                        ZoneCode = int.Parse(row[4]),
                        ZoneName = row[5],
                        ZonePopulation = int.Parse(row[6])
                    });
                    line++;
                }
                db.SaveChanges();
                Debug.WriteLine("{0} lines imported.", line);
            }
            db.SaveChanges();
            return null;
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