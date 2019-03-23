using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Repositories
{
    public class PostRepository : IPostRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public PostRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Post CreateOne(Post parameters)
        {
            this.context.Posts.Add(new Post()
            {
                ThreadID = parameters.ThreadID,
                Text = parameters.Text,
                TimeStamp = parameters.TimeStamp,
                creator = parameters.creator
            });
            this.context.SaveChanges();
            return parameters;
        }

        public void DeleteOneById(int id)
        {
            Post post = this.context.Posts.Where(x => x.Id == id).SingleOrDefault();
            this.context.Posts.Remove(post);
            this.context.SaveChanges();
        }

        public ICollection<Post> getAll()
        {
            return this.context.Posts.ToList();
        }

        public IEnumerable<Post> getAllByThreadId(int ThreadId)
        {
            return this.context.Posts.Where(x => x.ThreadID == ThreadId).ToList();
        }

        public Post GetOneById(int id)
        {
            return this.context.Posts.Where(x => x.Id == id).SingleOrDefault();
        }

        public Post UpdateOne(Post parameters)
        {
            this.context.Posts.AddOrUpdate(x => x.Id, parameters);
            this.context.SaveChanges();
            return parameters;
        }

    }
}
