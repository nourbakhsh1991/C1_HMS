using BMS.Infrastructure.Interfaces;
using BMS.Modbus.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace BMS.Services
{
    public class Startup : IStartupApplication
    {
        public int Priority => 4;

        public bool BeforeConfigure => false;

        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {

      
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
