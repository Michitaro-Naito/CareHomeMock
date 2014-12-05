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
                new License() { Name = "��t" },
                new License() { Name = "���Ȉ�t" },
                new License() { Name = "��܎t" },
                new License() { Name = "�ی��t" },
                new License() { Name = "���Y�t" },
                new License() { Name = "�Ō�t" },
                new License() { Name = "�y�Ō�t" },
                new License() { Name = "���w�Ö@�m" },
                new License() { Name = "��ƗÖ@�m" },
                new License() { Name = "���\�P���m" },
                new License() { Name = "���ȉq���m" },
                new License() { Name = "���꒮�o�m" },
                new License() { Name = "�Ǘ��h�{�m" },
                new License() { Name = "�h�{�m" },
                new License() { Name = "�`������m" },
                new License() { Name = "���񖀃}�b�T�[�W�w���t" },
                new License() { Name = "�͂�t" },
                new License() { Name = "���イ�t" },
                new License() { Name = "�_�������t" },
                new License() { Name = "�Љ���m" },
                new License() { Name = "��앟���m" },
                new License() { Name = "���_�ی������m" }
                );
        }
    }
}
