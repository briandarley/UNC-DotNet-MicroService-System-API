using System.Api.Infrastructure.Interfaces.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Serilog;
using UNC.Services;
using UNC.Services.Interfaces.Response;

namespace System.Api.Infrastructure.Services.Api
{
    public class AppPoolService:ServiceBase, IAppPoolService
    {
        public AppPoolService(ILogger logger) : base(logger)
        {
        }

        public Task<IResponse> GetAppPools()
        {
            try
            {
                LogBeginRequest();

                return Task.FromResult((IResponse)SuccessResponse());

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

                return Task.FromResult((IResponse)CollectionResponse(sites.Select(c => c.Name).ToList()));
                

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
