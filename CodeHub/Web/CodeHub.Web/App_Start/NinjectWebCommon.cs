[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CodeHub.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CodeHub.Web.App_Start.NinjectWebCommon), "Stop")]

namespace CodeHub.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using CodeHub.Data;
    using CodeHub.Data.Contracts;
    using CodeHub.Common.FileUpload;
    using CodeHub.Common.RandomGenerator.Contracts;
    using CodeHub.Common.RandomGenerator;
    using CodeHub.Web.Infrastructure.Populators;
    using CodeHub.Web.Infrastructure.Caching;

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
            kernel.Bind<ICodeHubDbContext>().To<CodeHubDbContext>();
            kernel.Bind<ICodeHubData>().To<CodeHubData>();

            kernel.Bind<IFileUploader>().To<FileUploadHelper>();
            kernel.Bind<ICacheService>().To<InMemoryCache>();
            kernel.Bind<IDropDownListPopulator>().To<DropDownListPopulator>();
        }        
    }
}
