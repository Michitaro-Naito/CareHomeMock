using CareHomeMock.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Globalization;

namespace CareHomeMock.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        UserManager<User> _userManager = null;

        /// <summary>
        /// Manages ASP.NET Users.
        /// </summary>
        public UserManager<User> UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));
                return _userManager;
            }
        }

        RoleManager<IdentityRole> _roleManager = null;

        /// <summary>
        /// Manages ASP.NET Roles.
        /// </summary>
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                if(_roleManager == null)
                    _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                return _roleManager;
            }
        }

        bool _queried = false;
        User _user = null;

        /// <summary>
        /// Gets a record of current User.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                if (!_queried)
                {
                    _user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    _queried = true;
                }
                return _user;
            }
        }

        // ----- Override -----
        public BaseController()
            : base()
        {
            var culture = CultureInfo.CreateSpecificCulture("ja-JP");
            //var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Shows a short message to Visitor on the next page.
        /// Uses Cookie.
        /// </summary>
        /// <param name="message"></param>
        protected void Flash(string message)
        {
            //var currentMessage = "";
            //var currentCookie = Request.Cookies["__FlashMessage"];
            //if (currentCookie != null)
            //    currentMessage += currentCookie.Value;
            //currentMessage += message;
            Response.Cookies.Add(new HttpCookie("__FlashMessage", HttpUtility.UrlEncode(message)));
        }

        /// <summary>
        /// Logs an user activity to TableStorage.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        protected void Log(LogType level, string action, object data = null, string userId = null)
        {
            userId = userId ?? User.Identity.Name;
            var log = new Log()
            {
                // Layer 4
                IpAddress = Request.UserHostAddress,

                // Layer 7
                HttpMethod = Request.HttpMethod,
                Url = Request.Url.ToString(),
                ContentLength = Request.ContentLength,
                Cookies = JsonConvert.SerializeObject(Request.Unvalidated.Cookies.ToDictionary(), Formatting.Indented),
                QueryString = JsonConvert.SerializeObject(Request.Unvalidated.QueryString.ToDictionary(), Formatting.Indented),
                Form = JsonConvert.SerializeObject(Request.Unvalidated.Form.ToDictionary(), Formatting.Indented),

                // Application Layer
                LogType = level,
                UserId = userId,
                Action = action,
                JsonData = JsonConvert.SerializeObject(data, Formatting.Indented)
            };
            TableHelper<Log>.Table.Insert(log);
        }

        /// <summary>
        /// Adds a review to CareManager
        /// </summary>
        /// <param name="careManagerId"></param>
        /// <param name="reviewerType"></param>
        /// <param name="rating"></param>
        /// <param name="comment"></param>
        /// <param name="reply"></param>
        protected void AddReview(int careManagerId, ReviewerType reviewerType, int rating, string comment, string reply = null)
        {
            var review = new Review()
            {
                // Layer 4
                IpAddress = Request.UserHostAddress,
                Host = Request.UserHostName,

                // Application Layer
                PartitionKey = careManagerId.ToString(),
                ReviewerType = reviewerType,
                Rating = rating,
                Comment = comment,
                Reply = reply
            };
            TableHelper<Review>.Table.Insert(review);
        }

        /// <summary>
        /// Sends an email using outer SMTP server.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        protected void SendEmail(string to, string subject, string body)
        {
            var from = "hospia-debug@amlitek.com";

            var message = new MailMessage(from, to, subject, body);
            message.BodyEncoding = System.Text.Encoding.GetEncoding(50220); // Shift-JIS
            var smtp = new SmtpClient("smtp.lolipop.jp", 587);
            smtp.Credentials = new NetworkCredential("hospia-debug@amlitek.com", "Qom72demean");
            smtp.Send(message);
        }

        /// <summary>
        /// Sends an email to Admin.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        protected void SendEmailToAdmin(string subject, string body)
        {
            SendEmail("m-naito@amlitek.com", subject, body);
        }
	}
}