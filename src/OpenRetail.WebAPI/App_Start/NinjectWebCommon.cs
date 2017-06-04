[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(OpenRetail.WebAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(OpenRetail.WebAPI.App_Start.NinjectWebCommon), "Stop")]

namespace OpenRetail.WebAPI.App_Start
{        
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;

    using log4net;
    using Ninject;
    using Ninject.Web.Common;

    using OpenRetail.Repository.Api;
    using OpenRetail.Repository.Service;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            try
            {
                NinjectHelper.Kernel = kernel;

                // Install our Ninject-based IDependencyResolver into the Web API config
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDapperContext>().To<DapperContext>().InRequestScope(); //  single instance
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            //Load log4net Configuration
            log4net.Config.XmlConfigurator.Configure();
            kernel.Bind<ILog>().ToMethod(c => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).InRequestScope();
        }
    }
}