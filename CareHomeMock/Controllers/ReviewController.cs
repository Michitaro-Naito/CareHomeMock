using CareHomeMock.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    /// <summary>
    /// CareManager manages Reviews here.
    /// </summary>
    [Authorize]
    public class ReviewController : BaseController
    {
        /// <summary>
        /// CareManager views reviews for him.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var careManager = CurrentUser.CareManager.FirstOrDefault();
            if (careManager == null)
                return HttpNotFound();
            return View(new ReviewIndexVM() { CareManagerId = careManager.CareManagerId });
        }

        /// <summary>
        /// Gets ReviewerTypes for select options.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetReviewerTypes()
        {
            var types = Enum.GetNames(typeof(ReviewerType)).Select(n => new { id = Enum.Parse(typeof(ReviewerType), n), name = n });
            return Json(new { reviewerTypes = types });
        }

        /// <summary>
        /// CareManager generates an OTP like "1234" to let his customer to review.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateOtp(ReviewerType reviewerType)
        {
            var careManager = CurrentUser.CareManager.FirstOrDefault();
            if (careManager == null)
                return HttpNotFound();

            // Clears expired OTPs.
            var now = DateTime.UtcNow;
            var otpsToRemove = careManager.Otps.Where(otp => otp.Expires < now).ToList();
            if (otpsToRemove.Count > 0)
            {
                db.Otps.RemoveRange(otpsToRemove);
                db.SaveChanges();
            }

            // Generates
            Otp newOtp = null;
            var random = new Random();
            for (var n = 0; n < 10; n++)
            {
                using (var db = new ApplicationDbContext())
                {
                    newOtp = new Otp()
                    {
                        CareManagerId = careManager.CareManagerId,
                        VerificationCode = string.Format("{0:0000}", random.Next(10000)),
                        Expires = DateTime.UtcNow.AddYears(1),
                        ReviewerType = reviewerType
                    };
                    db.Otps.Add(newOtp);
                    try
                    {
                        db.SaveChanges();
                        Log(LogType.CareManager, "OTPを生成しました。", new { newOtp.CareManagerId, newOtp.ReviewerType, newOtp.VerificationCode });
                        return Json(new { otp = newOtp });
                    }
                    catch (DbUpdateException)
                    {
                        // Continues to try another random code.
                    }
                }
            }

            // Failed to generate.
            return Json(new { otp = (Otp)null });
        }

        /// <summary>
        /// CareHome replies to a review.
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reply(string partitionKey, string rowKey, string message)
        {
            var careManager = CurrentUser.CareManager.FirstOrDefault(m => m.CareManagerId == int.Parse(partitionKey));
            if (careManager == null)
                return HttpNotFound();

            var table = TableHelper<Review>.Table;
            var review = table.CreateQuery<Review>().Where(r => r.PartitionKey == partitionKey && r.RowKey == rowKey).ToList().First();
            review.Reply = message;
            table.Execute(TableOperation.Replace(review));

            Log(LogType.CareManager, "評価に返信しました。", new { review.PartitionKey, review.RowKey });
            return Json(new { success = true });
        }

	}
}