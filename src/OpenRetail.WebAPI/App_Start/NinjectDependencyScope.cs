using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using System.Web.Http.Dependencies;

namespace OpenRetail.WebAPI.App_Start
{
    public class NinjectDependencyScope : IDependencyScope
    {
        protected IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            IRequest request = resolver.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolver.Resolve(request).SingleOrDefault();
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            IRequest request = resolver.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolver.Resolve(request).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
        }
    }
}