namespace CareHomeMock.Migrations
{
    using CareHomeMock.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CareHomeMock.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CareHomeMock.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // Populates Licenses
            context.Licenses.AddOrUpdate(
                l => l.Name,
                new License() { Name = "ˆãt" },
                new License() { Name = "•‰Èˆãt" },
                new License() { Name = "–òÜt" },
                new License() { Name = "•ÛŒ’t" },
                new License() { Name = "•Yt" },
                new License() { Name = "ŠÅŒìt" },
                new License() { Name = "yŠÅŒìt" },
                new License() { Name = "—Šw—Ã–@m" },
                new License() { Name = "ì‹Æ—Ã–@m" },
                new License() { Name = "‹”\ŒP—ûm" },
                new License() { Name = "•‰È‰q¶m" },
                new License() { Name = "Œ¾Œê’®Šom" },
                new License() { Name = "ŠÇ—‰h—{m" },
                new License() { Name = "‰h—{m" },
                new License() { Name = "‹`ˆ‘•‹ïm" },
                new License() { Name = "‚ ‚ñ–€ƒ}ƒbƒT[ƒWwˆ³t" },
                new License() { Name = "‚Í‚èt" },
                new License() { Name = "‚«‚ã‚¤t" },
                new License() { Name = "_“¹®•œt" },
                new License() { Name = "Ğ‰ï•Ÿƒm" },
                new License() { Name = "‰îŒì•Ÿƒm" },
                new License() { Name = "¸_•ÛŒ’•Ÿƒm" }
                );
        }
    }
}
