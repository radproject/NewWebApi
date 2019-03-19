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

        public UserRepository()
        {
            this.context = new ApplicationDbContext();
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
            return null;
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