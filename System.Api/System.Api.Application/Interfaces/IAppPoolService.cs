using System.Threading.Tasks;
using UNC.Services.Interfaces.Response;

namespace System.Api.Application.Interfaces
{
    public interface IAppPoolService
    {
        Task<IResponse> GetAppPools();
        Task<IResponse> GetSites();
    }
}
