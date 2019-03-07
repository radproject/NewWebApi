using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Repositories
{
    public class PostRepository : IRepository<Db.Post, int>
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public PostRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Db.Post CreateOne(Db.Post parameters)
        {
            throw new NotImplementedException();
        }

        public void DeleteOneById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Db.Post> getAll()
        {
            throw new NotImplementedException();
        }

        public Db.Post GetOneById(int id)
        {
            throw new NotImplementedException();
        }

        public Db.Post UpdateOne(Db.Post parameters)
        {
            throw new NotImplementedException();
        }
    }
}