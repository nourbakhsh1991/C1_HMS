using System;
using BMS.Domain.DataSettings;
using BMS.Domain.Interfaces;
using BMS.Domain.MongoDb;
using BMS.Infrastructure.Interfaces;
using BMS.Shared.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.Infrastructure.Startup
{
    public class StartupApplication : IStartupApplication
    {
        public int Priority => 1;

        public bool BeforeConfigure => false;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            CommonPath.BaseDirectory = webHostEnvironment.ContentRootPath;
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            CommonPath.BaseDirectory = hostingEnvironment.ContentRootPath;

            IConfigurationSection section = configuration.GetSection(nameof(DefaultSettings));
            section.Get<DefaultSettings>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IDatabaseContext, MongoDBContext>();
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

        }
    }
}
