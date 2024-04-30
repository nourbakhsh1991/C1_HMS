using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BMS.Domain.DataSettings;
using BMS.Domain.Interfaces;
using BMS.Infrastructure.Interfaces;
using BMS.Services.Files;
using BMS.Services.Files.Interfaces;
using BMS.Services.Menus;
using BMS.Services.Menus.Interfaces;
using BMS.Services.Permissions;
using BMS.Services.Permissions.Interfaces;
using BMS.Services.Roles;
using BMS.Services.Roles.Interfaces;
using BMS.Services.Users;
using BMS.Services.Users.Interfaces;
using BMS.Services.Map.Interfaces;
using BMS.Services.Map;
using BMS.Services.Modbus.Interfaces;
using BMS.Services.Modbus;
using MongoDB.Bson.Serialization;

namespace BMS.Services
{
    public class Startup : IStartupApplication
    {
        public int Priority => 3;

        public bool BeforeConfigure => false;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add Mondo discriminators

            BsonClassMap.RegisterClassMap<Domain.Geometry.Line.LineEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Line.LwPolyLineEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Arc.ArcEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Circle.CircleEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Circle.EllipseEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Insert.InsertEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Point.PointEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Text.MTextEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.GeometryEntity>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.SizeEntity>();
            BsonClassMap.RegisterClassMap<Domain.Models.Map.Entity>();
            BsonClassMap.RegisterClassMap<Domain.Models.Map.Block>();
            BsonClassMap.RegisterClassMap<Domain.Geometry.Block.BlockEntity>();


            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<ILayerService, LayerService>();
            services.AddScoped<IGeometryService, GeometryService>();
            services.AddScoped<IModbusService, ModbusService>();
            services.AddScoped<IModbusRegisterService, ModbusRegisterService>();
            services.AddScoped<IModbusHistoryService, ModbusHistoryService>();



            var provider = services.BuildServiceProvider();

            if (!DataSettingsManager.DatabaseIsInstalled())
            {
                var context = provider.GetRequiredService<IDatabaseContext>();
                context.CreateDatabase();

                InititalDataGenerator sampleData = new InititalDataGenerator();
                sampleData.Generate(provider);
            }

        }
    }
}
