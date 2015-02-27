using CareHomeMock.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Controllers
{
    public class EmailVerificationController : BaseController
    {
        /// <summary>
        /// CareManager verifies here.
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<ActionResult> Verify(string verificationCode)
        {
            var row = db.EmailVerifications.FirstOrDefault(v => v.VerificationCode == verificationCode);
            if (row == null)
            {
                Flash("無効なURLです。");
                return RedirectToAction("Index", "Home");
            }
            if (row.CareManager.User != null)
            {
                Flash("そのケアマネは既に認証されています。");
                return RedirectToAction("Index", "Home");
            }

            // Generates username and password.
            var username = string.Format("CM{0:D6}", row.CareManagerId);
            var password = Helper.PasswordHelper.GeneratePassword(12);

            // Registers CareHomeUser.
            var user = new User() { UserName = username, Email = row.Email, Name = row.CareManager.Name };
            var result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("会員を登録できませんでした。");

            var registeredUser = db.Users.FirstOrDefault(u => u.UserName == username);
            if (registeredUser == null)
                throw new Exception("登録されたはずの会員が見つかりませんでした。");

            // Notifies Sender.
            Flash("ケアマネ会員として認証されました。IDとパスワードを電子メールアドレスにお送りいたしましたのでご確認ください。");
            //SendEmail(registeredUser.Email, "[ケアマネ情報局] ケアマネ会員として認証されました", string.Format("ID:{0} password:{1}", username, password));
            dynamic email = new Postal.Email("EmailVerified");
            email.To = registeredUser.Email;
            email.CareHomeName = row.CareManager.CareHome.Name;
            email.CareManagerName = row.CareManager.Name;
            email.UserId = username;
            email.Password = password;
            email.SiteUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
            email.Send();

            // Adds CareManager and deletes EmailVerification.
            registeredUser.CareManager.Add(row.CareManager);
            db.EmailVerifications.Remove(row);
            db.SaveChanges();
            Log(LogType.CareManager, "ケアマネ会員としてメール認証しました。", new { row.Email, row.CareManagerId });

            return RedirectToAction("Index", "Home");
        }
	}
}