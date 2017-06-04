using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject;
using System.Web.Http.Dependencies;

namespace OpenRetail.WebAPI.App_Start
{
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }
}