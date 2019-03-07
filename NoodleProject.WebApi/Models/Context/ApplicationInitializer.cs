using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Context
{
    public class ApplicationInitializer: DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            SeedUsers(context);
            SeedTopics(context);

            base.Seed(context);
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);

            List<IdentityRole> identityRoles = new List<IdentityRole>();
            identityRoles.Add(new IdentityRole() { Name = "Admin" });
            identityRoles.Add(new IdentityRole() { Name = "Lecturer" });
            identityRoles.Add(new IdentityRole() { Name = "Student" });

            foreach (IdentityRole role in identityRoles)
            {
                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser admin = new ApplicationUser();
            admin.Email = "admin@noodle.com";
            admin.UserName = "admin@noodle.com";

            userManager.Create(admin, "noodleAdmin$1");
            userManager.AddToRole(admin.Id, "Admin");

            context.SaveChanges();
        }

        private void SeedTopics(ApplicationDbContext context)
        {
            ApplicationUser au = context.Users.Where(x => x.Email == "admin@noodle.com").SingleOrDefault();
            var threads = new List<Topic>
            {
                new Topic{Title = "Welcome to Noodle!", CreationDate = DateTime.Now, creator = au, subscribers = new List<ApplicationUser>() { au }, isPrivate = false, posts = new List<Db.Post> { new Db.Post() { creator = au, Text = "Hello everyone! This is the admin speaking! Welcome to Noodle, the better and faster version of Moodle! Here you can get to know the platform and ask any general questions. Enjoy the stay!" } } }
            };

            threads.ForEach(thread =>
            {
                context.Topics.Add(thread);
            });

            context.SaveChanges();
        }
    }
}