using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet]
        [ActionName("search")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search(SearchRequest searchRequest)
        {
            SearchResponse response = new();
            return Ok(response);
        }

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
