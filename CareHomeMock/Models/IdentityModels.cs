using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CareHomeMock.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<CareHome> CareHomes { get; set; }
        public DbSet<CareManager> CareManagers { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}