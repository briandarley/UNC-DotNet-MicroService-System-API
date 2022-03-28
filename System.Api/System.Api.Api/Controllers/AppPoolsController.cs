using System;
using System.Api.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UNC.API.Base;
using UNC.API.Base.Infrastructure;
using UNC.Services;

namespace System.Api.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "iis")]
    public class AppPoolsController:BaseController
    {
        private readonly IAppPoolService _service;

        public AppPoolsController(ILogService logService, System.Api.Application.Interfaces.IAppPoolService service) : base(logService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppPools()
        {
            try
            {
                LogBeginRequest();

                var request = await _service.GetAppPools();

                return request.ToActionResult();


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

        [HttpPost, Route("{name}/restart")]
        public async Task<IActionResult> RestartPool(string name)
        {
            try
            {
                LogBeginRequest();

                var response = await _service.RestartPool(name);

                return response.ToActionResult();

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

        [HttpPost, Route("{name}/stop")]
        public async Task<IActionResult> StopPool(string name)
        {
            try
            {
                LogBeginRequest();

                var response = await _service.StopPool(name);

                return response.ToActionResult();

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

        [HttpPost, Route("{name}/start")]
        public async Task<IActionResult> StartPool(string name)
        {
            try
            {
                LogBeginRequest();

                var response = await _service.StartPool(name);

                return response.ToActionResult();

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
