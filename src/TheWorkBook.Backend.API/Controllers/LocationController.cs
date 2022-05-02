using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    [Route("v{version:apiVersion}/location/[action]")]
    public class LocationController : BaseController
    {
        readonly ILocationService _locationService;

        public LocationController(ILogger<SearchController> logger,
            IEnvVariableHelper envVariableHelper,
            ILocationService locationService)
            : base(logger, envVariableHelper)
        {
            _locationService = locationService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("getCounties")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCounties()
        {
            List<LocationDto> counties = await _locationService.GetCountiesAsync();
            return Ok(counties);
        }
    }
}
