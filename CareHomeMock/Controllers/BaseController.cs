﻿using CareHomeMock.Models;
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

namespace CareHomeMock.Controllers
{
    public class BaseController : Controller
    {
        UserManager<User> _userManager = null;
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
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                if(_roleManager == null)
                    _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                return _roleManager;
            }
        }

        /// <summary>
        /// Shows a short message to Visitor on the next page.
        /// Uses Cookie.
        /// </summary>
        /// <param name="message"></param>
        protected void Flash(string message)
        {
            var currentMessage = "";
            var currentCookie = Request.Cookies["__FlashMessage"];
            if (currentCookie != null)
                currentMessage += currentCookie.Value;
            currentMessage += message;
            Response.Cookies.Add(new HttpCookie("__FlashMessage", currentMessage));
        }

        protected void Log(LogType level, string userId, string action, object data)
        {
            var log = new Log()
            {
                // Layer 4
                IpAddress = Request.UserHostAddress,

                // Layer 7
                HttpMethod = Request.HttpMethod,
                Url = Request.Url.ToString(),
                ContentLength = Request.ContentLength,
                Cookies = JsonConvert.SerializeObject(Request.Cookies.ToDictionary(), Formatting.Indented),
                QueryString = JsonConvert.SerializeObject(Request.QueryString.ToDictionary(), Formatting.Indented),
                Form = JsonConvert.SerializeObject(Request.Form.ToDictionary(), Formatting.Indented),

                // Application Layer
                LogType = level,
                UserId = userId,
                Action = action,
                JsonData = JsonConvert.SerializeObject(data, Formatting.Indented)
            };
            TableHelper<Log>.Table.Insert(log);
        }

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

        protected void SendEmail(string to, string subject, string body)
        {
            var from = "info@carehome.jp";

            var message = new MailMessage(from, to, subject, body);
            message.BodyEncoding = System.Text.Encoding.GetEncoding(50220); // Shift-JIS
            var smtp = new SmtpClient("hospia.jp", 587);
            smtp.Credentials = new NetworkCredential("info@hospia.jp", "mgr57553830");
            smtp.Send(message);
        }

        protected void SendEmailToAdmin(string subject, string body)
        {
            SendEmail("m-naito@amlitek.com", subject, body);
        }
	}
}