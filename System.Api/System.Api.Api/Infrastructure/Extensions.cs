using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Security.Principal;
using System.Api.Infrastructure.Services.Internals;
using UNC.API.Base.Infrastructure;
using UNC.HttpClient;
using UNC.HttpClient.Interfaces;
using UNC.HttpClient.Models;
using UNC.Services;
using UNC.Services.Infrastructure;

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
