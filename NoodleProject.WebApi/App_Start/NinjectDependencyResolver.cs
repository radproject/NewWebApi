using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Repositories;

namespace NoodleProject.WebApi.App_Start
{
    public class NinjectDependencyScope : IDependencyScope
    {
        IResolutionRoot ninjectResolver;

        public NinjectDependencyScope(IResolutionRoot ninjectResolver)
        {
            this.ninjectResolver = ninjectResolver;
        }

        public object GetService(Type serviceType)
        {
            if (this.ninjectResolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return this.ninjectResolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this.ninjectResolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return this.ninjectResolver.GetAll(serviceType);
        }

        public void Dispose()
        {
            IDisposable disposable = this.ninjectResolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            this.ninjectResolver = null;
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel) : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }
}