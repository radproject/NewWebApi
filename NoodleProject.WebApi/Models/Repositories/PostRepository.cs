using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        public Post CreateOne(Post parameters)
        {
            Post post = this.context.Posts.Add(parameters);
            this.context.SaveChanges();
            return post;
        }

        public void DeleteOneById(int id)
        {
            Post post = this.context.Posts.Where(x => x.ID == id).SingleOrDefault();
            this.context.Posts.Remove(post);
            this.context.SaveChanges();
        }

        public ICollection<Post> getAll()
        {
            return this.context.Posts.ToList();
        }

        public Post GetOneById(int id)
        {
            return this.context.Posts.Where(x => x.ID == id).SingleOrDefault();
        }

        public Post UpdateOne(Post parameters)
        {
            this.context.Posts.AddOrUpdate(x => x.ID, parameters);
            this.context.SaveChanges();
            return parameters;
        }
    }
}
