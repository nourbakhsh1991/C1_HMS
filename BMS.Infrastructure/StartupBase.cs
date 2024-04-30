
using BMS.Infrastructure.Configurations;
using BMS.Infrastructure.Interfaces;
using BMS.Shared.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Linq;
using BMS.Infrastructure.AppTypes;
using MediatR;

namespace BMS.Infrastructure
{
    /// <summary>
    /// Represents startup
    /// </summary>
    public static class StartupBase
    {
        #region Methods


        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //find startup configurations provided by other assemblies
            var typeSearcher = new AppTypeSearcher();
            services.AddSingleton<ITypeSearcher>(typeSearcher);

            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //Register startup
            var instancesBefore = startupConfigurations
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .Where(startup => startup.BeforeConfigure)
                .OrderBy(startup => startup.Priority);

            //configure services
            foreach (var instance in instancesBefore)
                instance.ConfigureServices(services, configuration);


            


            //Register startup
            var instancesAfter = startupConfigurations
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .Where(startup => !startup.BeforeConfigure)
                .OrderBy(startup => startup.Priority);

            //configure services
            foreach (var instance in instancesAfter)
                instance.ConfigureServices(services, configuration);

            //add mediator
            //AddMediator(services, typeSearcher);
        }

        /// <summary>
        /// Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="webHostEnvironment">WebHostEnvironment</param>
        public static void ConfigureRequestPipeline(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            //find startup configurations provided by other assemblies
            var typeSearcher = new AppTypeSearcher();
            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Priority);

            //configure request pipeline
            foreach (var instance in instances)
                instance.Configure(application, webHostEnvironment);
        }

        #endregion

        /// <summary>
        /// Adds services for mediatR
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddMediator(this IServiceCollection services, AppTypeSearcher typeSearcher)
        {
            var assemblies = typeSearcher.GetAssemblies().ToArray();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StartupBase).Assembly));
        }

    }


}
