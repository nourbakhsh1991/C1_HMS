using BMS.Domain.BaseModels;
using BMS.Api.Hubs;
using BMS.Modbus.Jobs;
using BMS.Services.Modbus.Interfaces;
using BMS.Services.Modbus;
using MathNet.Numerics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Quartz;
using System.Text;
using BMS.Api.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using BMS.Modbus.Services;

namespace BMS.Api
{
    public class Program
    {
        readonly static string MyAllowSpecificOrigins = "AllowOrigin";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    // Setup a HTTP/2 endpoint without TLS.
            //   // options.ListenAnyIP(5268, o => o.Protocols = HttpProtocols.Http2);
            //    options.ListenAnyIP(7199, o => o.Protocols = HttpProtocols.Http1);
            //    options.ListenAnyIP(7273, o => o.Protocols = HttpProtocols.Http2);
            //});

            // Add services to the container.
            builder.Services.AddCors(c =>
            {
                //c.AddPolicy(MyAllowSpecificOrigins,
                //options => options.AllowAnyOrigin()
                //.AllowAnyHeader()
                //                      .AllowAnyMethod());
                //c.AddDefaultPolicy(b =>
                //{
                //    b.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                //});
                //c.AddPolicy("SIGNALR",
                //    new CorsPolicyBuilder()
                //    .WithOrigins("http://localhost:4200")
                //    .AllowAnyHeader()
                //    .AllowAnyMethod()
                //    .AllowCredentials()
                //    .Build());
                c.AddPolicy(MyAllowSpecificOrigins,
                   builder =>
                   {
                       builder.SetIsOriginAllowed(_ => true);
                       builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                   });
            });



            var appAuthSection = builder.Configuration.GetSection("AppAuth");
            builder.Services.Configure<AppAuth>(appAuthSection);
            var appAuth = appAuthSection.Get<AppAuth>();
            var key = Encoding.UTF8.GetBytes(appAuth.Secret);
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.RequireHttpsMetadata = false;
                op.SaveToken = false;
                op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            //builder.Services.AddGrpc();

            builder.Services.AddSignalR();

            

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Infrastructure.StartupBase.ConfigureServices(builder.Services, builder.Configuration);

            var sp = builder.Services.BuildServiceProvider();


            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                // Just use the name of your job that you created in the Jobs folder.
                var jobKey = new JobKey("ModbusHandleJob");
                q.AddJob<ModbusHandleJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("ModbusHandleJob-trigger")
                    //This Cron interval can be described as "run every minute" (when second is zero)
                    .WithCronSchedule("* * * ? * *")
                );

                var jobStarterKey = new JobKey("ModbusStarterJob");
                q.AddJob<ModbusStarterJob>(opts => opts.WithIdentity(jobStarterKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobStarterKey)
                    .WithIdentity("ModbusStarterJob-trigger")
                    //This Cron interval can be described as "run every minute" (when second is zero)
                    .WithCronSchedule("0 * * ? * * *")
                );
            });

            builder.Services.AddHostedService<MqttDataHandlerService>();

            var app = builder.Build();


            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            //app.UseCors("SIGNALR");

            Infrastructure.StartupBase.ConfigureRequestPipeline(app, app.Environment);

            app.UseAuthentication();
            app.UseAuthorization();

            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".dxf"] = "application/dxf";
            provider.Mappings[".json"] = "application/json";

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
                RequestPath = new PathString("/Files"),
                ContentTypeProvider = provider
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ModbusDataHub>("/signalr/modbus/data");
                //endpoints.MapGrpcService<BMSDataCollectorService>() ;
                endpoints.MapControllers();

            });

            app.Run();
        }
    }
}