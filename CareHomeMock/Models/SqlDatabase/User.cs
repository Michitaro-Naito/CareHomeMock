using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    /// <summary>
    /// User model. Can be Admin, CareHome or CareManager.
    /// User hasMany CareHomes.
    /// User hasMany CareManagers.
    /// </summary>
    public class User : Microsoft.AspNet.Identity.EntityFramework.IdentityUser
    {
        public virtual ICollection<CareHome> CareHomes { get; set; }

        public virtual ICollection<CareManager> CareManager { get; set; }



        public User()
        {
            CareHomes = new List<CareHome>();
            CareManager = new List<CareManager>();
        }
    }
}