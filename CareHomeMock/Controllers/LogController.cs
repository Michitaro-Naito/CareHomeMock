using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using CareHomeMock.Models;

namespace CareHomeMock.Controllers
{
    /// <summary>
    /// Admin controls Logs here.
    /// </summary>
    public class LogController : BaseController
    {
        //
        // GET: /Log/
        public ActionResult Index()
        {
            /*var table = TableHelper<Log>.Table;

            //Log(LogType.CareHome, "Foo", "ログイン", null);

            var logs = table.CreateQuery<Log>()
                .Where(l => l.PartitionKey == "CareHome"
                    //&& l.RowKey.CompareTo("2519849844367280569_e3c286ec-8129-417c-a2e7-879f54917f44") > 0
                    //&& l.RowKey.CompareTo("2519849846612192995_981ae14c-c068-44e8-990f-3519a99d50b4") < 0
                    )
                .Take(10)
                .ToList();*/

            return View();
        }

        [HttpPost]
        public ActionResult GetLogs(LogType level, string rowKeyAfter = null)
        {
            var query = TableHelper<Log>.Table.CreateQuery<Log>()
                .Where(l => l.PartitionKey == level.ToString());
            if (rowKeyAfter != null)
                query = query.Where(l => l.RowKey.CompareTo(rowKeyAfter) > 0);
            return Json(query.Take(50).ToList());
        }
	}
}