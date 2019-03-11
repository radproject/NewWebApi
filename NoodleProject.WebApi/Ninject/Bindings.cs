using Ninject.Modules;
using NoodleProject.WebApi.Models.Db;
using NoodleProject.WebApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoodleProject.WebApi.Models.Context;

namespace NoodleProject.WebApi.Ninject
{
    public class Bindings: NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Topic, int>>().To<TopicRepository>();
            Bind<IRepository<NoodleProject.WebApi.Models.Db.Post, int>>().To<PostRepository>();
        }
    }
}