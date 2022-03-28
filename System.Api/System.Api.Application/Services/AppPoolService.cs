using System;
using System.Api.Application.Interfaces;
using System.Threading.Tasks;
using Serilog;
using UNC.Services;
using UNC.Services.Interfaces.Response;

namespace System.Api.Application.Services
{
    public class AppPoolService:ServiceBase, IAppPoolService
    {
        private readonly System.Api.Infrastructure.Interfaces.Services.IAppPoolService _service;

        public AppPoolService(ILogger logger, System.Api.Infrastructure.Interfaces.Services.IAppPoolService service) : base(logger)
        {
            _service = service;
        }

        public async Task<IResponse> GetAppPools()
        {
            try
            {
                LogBeginRequest();

                var request = await _service.GetAppPools();

                return request;
            }
            catch (Exception ex)
            {
                return LogException(ex, false);
            }
            finally
            {
                LogEndRequest();
            }
        }

        public async Task<IResponse> GetSites()
        {
            try
            {
                LogBeginRequest();

                return await _service.GetSites();
            }
            catch (Exception ex)
            {
                return LogException(ex, false);
            }
            finally
            {
                LogEndRequest();
            }
        }

        public async Task<IResponse> RestartPool(string name)
        {
            try
            {
                LogBeginRequest();

                return await _service.RestartPool(name);
            }
            catch (Exception ex)
            {
                return LogException(ex, false);
            }
            finally
            {
                LogEndRequest();
            }
        }

        public async Task<IResponse> StopPool(string name)
        {
            try
            {
                LogBeginRequest();

                return await _service.StopPool(name);
            }
            catch (Exception ex)
            {
                return LogException(ex, false);
            }
            finally
            {
                LogEndRequest();
            }
        }

        public async Task<IResponse> StartPool(string name)
        {
            try
            {
                LogBeginRequest();

                return await _service.StartPool(name);
            }
            catch (Exception ex)
            {
                return LogException(ex, false);
            }
            finally
            {
                LogEndRequest();
            }
        }
    }
}
