using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Repositories
{
    public class TopicRepository :ITopicRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public TopicRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Topic CreateOne(Topic parameters)
        {
            Topic topic = this.context.Topics.Add(parameters);
            this.context.SaveChanges();
            return parameters;
        }

        public void DeleteOneById(int id)
        {
            Topic topic = this.context.Topics.Where(t => t.ID == id).SingleOrDefault();
            this.context.Topics.Remove(topic);
            this.context.SaveChanges();
        }

        public ICollection<Topic> getAll()
        {
            return this.context.Topics.Where(t => (t.isPrivate == false)).ToList();
        }

        public IEnumerable<Topic> getAllForUserId(string UserId)
        {
            return this.context.Topics
                .Where(t => (
                    (t.creator.Id == UserId)
                    ||
                    ((t.subscribers.Where(s => (s.Id == UserId)).Count()) > 0)
                )).ToList();
        }

        public Topic GetOneById(int id)
        {
            Topic tempTopic = this.context.Topics.Where(t => t.ID == id).SingleOrDefault();
            tempTopic.posts = this.context.Posts.Where(p => p.ThreadID == id).ToArray();
            return tempTopic;
        }

        public Topic UpdateOne(Topic parameters)
        {
            this.context.Topics.AddOrUpdate(t => t.ID, parameters);
            this.context.SaveChanges();
            return parameters;
        }
    }
}