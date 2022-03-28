using System.Api.Infrastructure.Pocos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Security.Principal;
using System.Api.Infrastructure.Services.Internals;
using System.Collections.Generic;
using System.Linq;
using UNC.API.Base.Infrastructure;
using UNC.Extensions.General;
using UNC.HttpClient;
using UNC.HttpClient.Interfaces;
using UNC.HttpClient.Models;
using UNC.Services;
using UNC.Services.Infrastructure;
using UNC.Services.Interfaces.Response;

namespace System.Api.Api.Infrastructure
{
    public static class Extensions
    {

        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            UNC.HttpClient.Extensions.Extensions.RegisterDependencies(services);
            System.Api.Infrastructure.Services.Mapper.MapperService.RegisterMappings();


            RegisterApplicationServices(services);
            RegisterInfrastructureServices(services);
            RegisterRepositories(services);



            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("iis", new OpenApiInfo { Title = "Iis Controller", Version = "v1", Description = "API Endpoints for managing IIS on server" });



            });





            services.RegisterApiVersioning();
            services.RegisterDefaultCors();
            services.RegisterDefaultMvcSerialization();

            services.RegisterApiEndPoints(configuration);
            services.RegisterCustomLogging(configuration, LogTypes.ConsoleLogging | LogTypes.FileLogging | LogTypes.RemoteApiLogging);
            services.RegisterLogHttpClient(configuration);
            services.RegisterAutoMapper<AutoMapperService>();
            services.AddTransient<ILogService, LogService>();


            services.AddHttpContextAccessor();



            services.AddTransient<IWebClient>(cfg =>
            {
                var clientId = configuration.GetValue<string>("ClientId");
                var application = configuration.GetValue<string>("Application");

                var authSettings = configuration.GetSection("IdentityConnection").Get<IdentityConnection>();


                var client = new WebClient(cfg.GetService<Serilog.ILogger>(), clientId, application, authSettings, cfg.GetService<IPrincipal>());
                client.TokenRefreshed += (sender, args) =>
                {


                };
                return client;
            });


            services.AddSingleton(cfg =>
            {
                var node = configuration.GetSection("Endpoints").GetChildren();
                var appResources = (IApiResources)new UNC.HttpClient.Models.ApiResources();
                foreach (var section in node)
                {
                    var resource = new UNC.HttpClient.Models.ApiResource { Address = section.Value, Name = section.Key };
                    appResources.Resources.Add(resource);
                }

                return appResources;

            });
            services.AddSingleton(cfg =>
            {
                var dataService = cfg.GetRequiredService<System.Api.Infrastructure.Interfaces.Services.IDalDataService>();
                var settingsRequest = dataService.GetSettings();

                settingsRequest.GetAwaiter().GetResult();

                var settings = ((ICollectionResponse<Setting>)settingsRequest.Result).Entities;
                return settings.ToList();
            });
            services.AddSingleton<System.Api.Domain.Models.SwaggerDefaultPathSettingModel>(cfg =>
            {
                var environment = configuration.GetSection("Environment").Value;
                var settings = cfg.GetRequiredService<List<Setting>>();

                var model = new System.Api.Domain.Models.SwaggerDefaultPathSettingModel();
                var setting = settings.Single(c =>
                    c.AppDomain.EqualsIgnoreCase("Environment")
                    && c.Overload.EqualsIgnoreCase(environment)).Value;
                model.DefaultServicePath = setting;
                return model;

            });

            RegisterInfrastructureDependencies(services);
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {

            services.AddSingleton<UNC.Services.Utilities.Encryption>();
            services.AddSingleton<System.Api.Application.Interfaces.IAppPoolService, System.Api.Application.Services.AppPoolService>();



        }

        private static void RegisterInfrastructureServices(IServiceCollection services)
        {
            services.AddSingleton<System.Api.Infrastructure.Interfaces.Services.IDalDataService, System.Api.Infrastructure.Services.Api.DalDataService>();
            services.AddSingleton<System.Api.Infrastructure.Interfaces.Services.IAppPoolService, System.Api.Infrastructure.Services.Api.AppPoolService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {


        }


        private static void RegisterInfrastructureDependencies(IServiceCollection services)
        {



        }

    }
}
