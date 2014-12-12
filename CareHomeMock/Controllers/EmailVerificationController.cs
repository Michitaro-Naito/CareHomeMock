using CareHomeMock.Models;
using System;
using System.Collections.Generic;
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
            var username = "caremanager" + row.CareManagerId;
            var password = System.Web.Security.Membership.GeneratePassword(12, 1);

            // Registers CareHomeUser.
            var user = new User() { UserName = username, Email = row.Email, Name = row.CareManager.Name };
            var result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("会員を登録できませんでした。");

            var registeredUser = db.Users.FirstOrDefault(u => u.UserName == username);
            if (registeredUser == null)
                throw new Exception("登録されたはずの会員が見つかりませんでした。");

            // Adds CareManager and deletes EmailVerification.
            registeredUser.CareManager.Add(row.CareManager);
            db.EmailVerifications.Remove(row);
            db.SaveChanges();

            // Notifies Sender.
            SendEmail(registeredUser.Email, "[ケアマネ情報局] ケアマネ会員として承認されました", string.Format("ID:{0} password:{1}", username, password));

            return RedirectToAction("Index", "Home");
        }
	}
}