using Microsoft.AspNet.Identity.EntityFramework;
using NoodleProject.WebApi.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Repositories
{
    public class UserRepository : IRepository<ApplicationUser, string>
    {
        ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public ApplicationUser CreateOne(ApplicationUser parameters)
        {
            return null;
        }

        public void DeleteOneById(string id)
        {
            ApplicationUser user = this.context.Users.Where(x => x.Id == id).SingleOrDefault();
            this.context.Users.Remove(user);
            this.context.SaveChanges();
        }

        public ICollection<ApplicationUser> getAll()
        {
            return this.context.Users.ToList();
        }

        public ApplicationUser GetOneById(string id)
        {
            return this.context.Users.Where(x => x.Id == id).SingleOrDefault();
        }

        public ApplicationUser UpdateOne(ApplicationUser parameters)
        {
            this.context.Users.AddOrUpdate(x => x.Id, parameters);
            return parameters;
        }
    }
}