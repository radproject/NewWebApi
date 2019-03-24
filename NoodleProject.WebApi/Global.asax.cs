using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using NoodleProject.WebApi.App_Start;
using NoodleProject.WebApi.Models;
using NoodleProject.WebApi.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using NoodleProject.WebApi.Models.Repositories;

namespace NoodleProject.WebApi
{
    public class WebApiApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            // This is where we tell Ninject how to resolve service requests
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<ITopicRepository>().To<TopicRepository>().InRequestScope();
            kernel.Bind<IPostRepository>().To<PostRepository>().InRequestScope();
            kernel.Bind<IRepository<ApplicationUser, string>>().To<UserRepository>().InRequestScope();
        }

    }
}
