using System.Threading.Tasks;
using UNC.Services.Interfaces.Response;

namespace System.Api.Infrastructure.Interfaces.Services
{
    public interface IAppPoolService
    {
        Task<IResponse> GetAppPools();
        Task<IResponse> GetSites();
        Task<IResponse> RestartPool(string name);
        Task<IResponse> StopPool(string name);
        Task<IResponse> StartPool(string name);
    }
}
