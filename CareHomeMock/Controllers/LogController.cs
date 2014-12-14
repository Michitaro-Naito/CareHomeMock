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
using CareHomeMock.Helper;

namespace CareHomeMock.Controllers
{
    /// <summary>
    /// Admin controls Logs here.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class LogController : BaseController
    {
        //
        // GET: /Log/
        public ActionResult Index()
        {
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

        [HttpPost]
        public ActionResult DeleteLogs()
        {
            var rowKeyOneMonthAgo = Helper.Helper.GenerateRowKey(DateTime.UtcNow.AddMonths(-1));

            var table = TableHelper<Log>.Table;
            var batch = new TableBatchOperation();
            var logsToDelete = table.CreateQuery<Log>()
                .Where(l => l.RowKey.CompareTo(rowKeyOneMonthAgo) >= 0)
                .Take(50).ToList();
            if (logsToDelete.Count > 0)
            {
                foreach (var log in logsToDelete)
                {
                    batch.Delete(log);
                }
                table.ExecuteBatch(batch);
            }
            Log(LogType.Admin, "古いログを削除しました。");
            return Json(new { Deleted = logsToDelete.Count });
        }
	}
}