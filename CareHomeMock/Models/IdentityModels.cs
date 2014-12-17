using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CareHomeMock.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<CareHome> CareHomes { get; set; }
        public DbSet<CareManager> CareManagers { get; set; }
        public DbSet<CareManagerLicenses> CareManagerLicenses { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<Css> Csses { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<ReviewRating> ReviewRatings { get; set; }
        public DbSet<StaticPage> StaticPage { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}