using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Repositories
{
    public class TopicRepository : IRepository<Topic, int>
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public TopicRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Topic CreateOne(Topic parameters)
        {
            throw new NotFiniteNumberException(0000000.0000);
        }

        public void DeleteOneById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Topic> getAll()
        {
            throw new NotImplementedException();
        }

        public Topic GetOneById(int id)
        {
            throw new NotImplementedException();
        }

        public Topic UpdateOne(Topic parameters)
        {
            throw new NotImplementedException();
        }
    }
}