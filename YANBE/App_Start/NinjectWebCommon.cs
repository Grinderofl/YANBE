using System.Data.Entity;
using System.Linq;
using System.Web.Configuration;
using AutoMapper;
using Core.Domain;
using EFConvention;
using Ninject.Extensions.Conventions;
using Pygments;
using YANBE.Controllers;

[assembly: WebActivator.PreApplicationStartMethod(typeof(YANBE.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(YANBE.App_Start.NinjectWebCommon), "Stop")]

namespace YANBE.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAutoContextFactory>()
                .To<AutoContextFactory>()
                .InSingletonScope()
                .OnActivation(x => x.AddAssemblyContaining<Post>().AddAssemblyContaining<HomeController>().AddEntitiesBasedOn<Entity>());
            kernel.Bind<DbContext>()
                .ToMethod(x => kernel.Get<IAutoContextFactory>().Context()).InRequestScope()
                .WithConstructorArgument("connectionString",
                    x => WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            kernel.Bind(
                x =>
                    x.FromAssemblyContaining<PostController>()
                        .SelectAllClasses()
                        .InheritedFrom<Profile>()
                        .BindBase()
                        .Configure(c => c.InSingletonScope()));
            kernel.Bind<Highlighter>().ToSelf().InSingletonScope();
            Mapper.Initialize(x =>
            {
                var profiles =
                    typeof (PostController).Assembly.GetTypes()
                        .Where(t => typeof (Profile).IsAssignableFrom(t))
                        .Union(typeof (Post).Assembly.GetTypes().Where(c => typeof (Profile).IsAssignableFrom(c)));
                foreach (var profile in profiles)
                    x.AddProfile(kernel.GetService(profile) as Profile);
                x.ConstructServicesUsing(type => kernel.Get(type));
            });
        }        
    }
}
