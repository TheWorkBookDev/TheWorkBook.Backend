using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorkBook.Shared.Dto;
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

        // GET api/listings
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
