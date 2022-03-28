using System.Api.Domain.Models;
using System.Api.Domain.Models.AppPools;
using System.Api.Infrastructure.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Web.Administration;
using Serilog;
using UNC.Extensions.General;
using UNC.Services;
using UNC.Services.Interfaces.Response;

namespace System.Api.Infrastructure.Services.Api
{
    public class AppPoolService:ServiceBase, IAppPoolService
    {
        private readonly SwaggerDefaultPathSettingModel _swaggerDefaults;


        public AppPoolService(ILogger logger, System.Api.Domain.Models.SwaggerDefaultPathSettingModel swaggerDefaults) : base(logger)
        {
            _swaggerDefaults = swaggerDefaults;
        }

        public Task<IResponse> GetAppPools()
        {
            try
            {
                LogBeginRequest();


                var server = new ServerManager();

                var list = new List<AppPoolStateModel>();

                foreach (var appPool in server.ApplicationPools)
                {
                    list.Add(new AppPoolStateModel
                    {
                        Name = appPool.Name,
                        State = appPool.State.ToString(),
                        
                    });
                }
                

                return Task.FromResult(CollectionResponse(list));
               

            }
            catch (Exception ex)
            {
                return Task.FromResult((IResponse)LogException(ex, false));
            }
            finally
            {
                LogEndRequest();
            }
        }

        public  Task<IResponse> GetSites()
        {
            try
            {
                LogBeginRequest();

                var server = new ServerManager();
                var sites = server.Sites;

                var list = new List<SiteModel>();
                foreach (var site in sites)
                {
                    
                    foreach (var app in site.Applications)
                    {
                        
                        list.Add(new SiteModel
                        {
                            Name = site.Name,
                            State = site.State.ToString(),

                            Applications = new List<ApplicationModel>
                            {
                                new ApplicationModel
                                {
                                    Path = app.Path,
                                    
                                    SwaggerPath = $"{_swaggerDefaults.DefaultServicePath}{app.Path}/swagger/index.html",
                                    AppPool = new AppPoolStateModel
                                    {
                                        Name = app.ApplicationPoolName,
                                        State = server.ApplicationPools.Single(c=> c.Name.Equals(app.ApplicationPoolName)).State.ToString()
                                    }
                                }
                            }

                        });
                    }
                    
                }


                return Task.FromResult((IResponse)CollectionResponse(list));
                

            }
            catch (Exception ex)
            {
                return Task.FromResult((IResponse)LogException(ex, false));
            }
            finally
            {
                LogEndRequest();
            }
        }

        public Task<IResponse> RestartPool(string name)
        {
            try
            {
                LogBeginRequest();

                var server = new ServerManager();

                var appPool = server.ApplicationPools.FirstOrDefault(c => c.Name.EqualsIgnoreCase(name));

                if (appPool is null) return Task.FromResult(NotFoundResponse());

                appPool.Recycle();

                return Task.FromResult((IResponse)SuccessResponse(appPool.State.ToString()));
                
            }
            catch (Exception ex)
            {
                return Task.FromResult((IResponse)LogException(ex, false));
            }
            finally
            {
                LogEndRequest();
            }
        }

        public Task<IResponse> StopPool(string name)
        {
            try
            {
                LogBeginRequest();

                var server = new ServerManager();

                var appPool = server.ApplicationPools.FirstOrDefault(c => c.Name.EqualsIgnoreCase(name));

                if (appPool is null) return Task.FromResult(NotFoundResponse());

                appPool.Stop();

                return Task.FromResult((IResponse)SuccessResponse(appPool.State.ToString()));

            }
            catch (Exception ex)
            {
                return Task.FromResult((IResponse)LogException(ex, false));
            }
            finally
            {
                LogEndRequest();
            }
        }

        public Task<IResponse> StartPool(string name)
        {
            try
            {
                LogBeginRequest();

                var server = new ServerManager();

                var appPool = server.ApplicationPools.FirstOrDefault(c => c.Name.EqualsIgnoreCase(name));

                if (appPool is null) return Task.FromResult(NotFoundResponse());

                appPool.Start();

                return Task.FromResult((IResponse)SuccessResponse(appPool.State.ToString()));

            }
            catch (Exception ex)
            {
                return Task.FromResult((IResponse)LogException(ex, false));
            }
            finally
            {
                LogEndRequest();
            }
        }
    }
}
