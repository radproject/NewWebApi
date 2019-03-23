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

            PasswordHasher ps = new PasswordHasher();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);




            ApplicationUser admin = new ApplicationUser()
            {
                FullName = "noodle admin",
                Email = "admin@noodle.com",
                UserName = "admin@noodle.com",
                PasswordHash = ps.HashPassword("administrator$1"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            context.Users.Add(admin);

            ApplicationUser Lecturer = new ApplicationUser()
            {
                FullName = "Paul Pawell",
                Email = "PaulP@noodle.com",
                UserName = "PaulPnoodle.com",
                PasswordHash = ps.HashPassword("Paul2019"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()

                
            };
            context.Users.Add(Lecturer);


            ApplicationUser Student = new ApplicationUser()
            {
                FullName = "Karolis Gunka",
                Email = "karolis.gunka@mail.itlsigo.ie",
                UserName = "karolis.gunka@mail.itlsigo.ie",
                PasswordHash = ps.HashPassword("Gunka2019"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = "S00157347"
            };
            context.Users.Add(Student);


            ApplicationUser Student1 = new ApplicationUser()
            {
                FullName = "Alan Jachimczak",
                Email = "Alan.Jackimczak@mail.itlsigo.ie",
                UserName = "Alan.Jackimczak@mail.itlsigo.ie",
                PasswordHash = ps.HashPassword("Alan2019"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = "S00168420"

            };
            context.Users.Add(Student1);

            context.SaveChanges();

            admin = userManager.FindByEmail(admin.Email);
            Lecturer = userManager.FindByEmail(Lecturer.Email);
            Student = userManager.FindByEmail(Student.Email);
            Student1 = userManager.FindByEmail(Student1.Email);

            userManager.AddToRole(admin.Id, "Admin");
            userManager.AddToRole(Lecturer.Id, "Lecturer");
            userManager.AddToRole(Student.Id, "Student");
            userManager.AddToRole(Student1.Id, "Student");

            context.SaveChanges();

        }

        private void SeedTopics(ApplicationDbContext context)
        {
            ApplicationUser au = context.Users.Where(x => x.Email == "admin@noodle.com").SingleOrDefault();
            ApplicationUser Lecturer = context.Users.Where(x => x.Email == "PaulP@noodle.com").SingleOrDefault();
            ApplicationUser Student= context.Users.Where(x => x.Email == "karolis.gunka@mail.itlsigo.ie").SingleOrDefault();
            ApplicationUser Student1 = context.Users.Where(x => x.Email == "Alan.Jackimczak@mail.itlsigo.ie").SingleOrDefault();

            var threads = new List<Topic>
            {
                new Topic{Title = "Welcome to Noodle!", CreationDate = DateTime.Now, creator = au, subscribers = new List<ApplicationUser>() { au, Lecturer, Student, Student1 }, isPrivate = false, posts = new List<Db.Post> { new Db.Post() { creator = au, Text = "Hello everyone! This is the admin speaking! Welcome to Noodle, the better and faster version of Moodle! Here you can get to know the platform and ask any general questions. Enjoy the stay!" } } } ,
                new Topic{Title = "Its a private topic", CreationDate = DateTime.Now, creator = Student, subscribers = new List<ApplicationUser>() {Student, Lecturer }, isPrivate = true, posts = new List<Db.Post>{new Db.Post() { creator = Student, Text = "Hello this is a private and confidential topic that the student and lecturer can discuss in private"} } },
                new Topic{Title = "How you guys thing about the course?", CreationDate = DateTime.Now, creator = Student1, subscribers = new List<ApplicationUser>() {Student1, Student}, isPrivate = false, posts = new List<Db.Post>{new Db.Post() { creator = Student1, Text = "Hi, in here you guys can discuss about your course and share what you like or dislike about it!"} } }
                
            };

            

            threads.ForEach(thread =>
            {
                context.Topics.Add(thread);
            });

            context.SaveChanges();
        }
    }
}