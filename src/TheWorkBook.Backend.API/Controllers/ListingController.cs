using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    [Route("v{version:apiVersion}/listing/[action]")]
    public class ListingController : BaseController
    {
        public ListingController(ILogger<ListingController> logger,
            IEnvVariableHelper envVariableHelper)
            : base(logger, envVariableHelper)
        {

        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpPost]
        [ActionName("add")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(ListingDto listingInfo)
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("get")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid identifier)
        {
            ListingDto listing = new ListingDto();
            return Ok(listing);
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpPatch]
        [ActionName("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] JsonPatchDocument<ListingDto> patchDocListingDto)
        {
            return Ok();
        }
    }
}
