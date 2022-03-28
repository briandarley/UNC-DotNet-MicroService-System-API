
using System.Threading.Tasks;
using UNC.Services.Interfaces.Response;

namespace System.Api.Infrastructure.Interfaces.Services
{
    public interface IDalDataService
    {
        Task<IResponse> GetSettings();
    }
}
