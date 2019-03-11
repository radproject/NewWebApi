using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Repositories
{
    public interface ITopicRepository: IRepository<Topic, int>
    {
        IEnumerable<Topic> getAllForUserId(string UserId);
    }
}