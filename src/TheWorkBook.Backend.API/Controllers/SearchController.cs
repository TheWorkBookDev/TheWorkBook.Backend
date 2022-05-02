using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.ServiceModels;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    [Route("v{version:apiVersion}/search/[action]")]
    public class SearchController : BaseController
    {
        readonly ISearchService _searchService;

        public SearchController(ILogger<SearchController> logger,
            IEnvVariableHelper envVariableHelper,
            ISearchService searchService) : base(logger, envVariableHelper)
        {
            _searchService = searchService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("search")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search(SearchRequest searchRequest)
        {
            SearchResponse response = await _searchService.SearchListings(searchRequest);
            return Ok(response);
        }
    }
}
