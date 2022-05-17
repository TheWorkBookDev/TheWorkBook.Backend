using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    [Route("v{version:apiVersion}/listing/[action]")]
    public class ListingController : BaseController
    {
        readonly IListingService _listingService;

        public ListingController(ILogger<ListingController> logger,
            IEnvVariableHelper envVariableHelper,
            IListingService listingService) : base(logger, envVariableHelper)
        {
            _listingService = listingService;
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpPost]
        [ActionName("add")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromServices]IApplicationUser applicationUser, 
            [FromBody]NewListingDto listingDto)
        {
            await _listingService.AddListingAsync(applicationUser.UserId.Value, listingDto);
            return Ok(listingDto);  // Need to return the id of the listing.
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("get")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            ListingDto listingDto = await _listingService.GetListingAsync(id);
            return Ok(listingDto);
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpGet]
        [ActionName("getMyListings")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ListingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyListings()
        {
            IEnumerable<ListingDto> listings = await _listingService.GetMyListingsAsync();
            return Ok(listings);
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpPatch]
        [ActionName("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromServices] IApplicationUser applicationUser,
            [FromBody] JsonPatchDocument<ListingDto> patchDocListingDto, int listingId)
        {
            if (patchDocListingDto == null) return BadRequest();

            // Check that user is entitled to update this listing.
            await CheckUserAuthorized(listingId, applicationUser);

            await _listingService.UpdateListingAsync(listingId, patchDocListingDto);

            return Ok();
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpPost]
        [ActionName("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromServices] IApplicationUser applicationUser,
            [FromBody] ListingDto listingDto)
        {
            // Check that user is entitled to update this listing.
            await CheckUserAuthorized(listingDto.ListingId, applicationUser);

            await _listingService.UpdateListingAsync(listingDto);
            return Ok();
        }

        private async Task CheckUserAuthorized(int listingId, IApplicationUser user)
        {
            // Check that user is entitled to update this listing.
            ListingDto listing = await _listingService.GetListingAsync(listingId);
            
            if (listing.UserId != user.UserId)
            {
                throw new System.Security.SecurityException("User is not authorized to update this listing.");
            }
        }
    }
}
