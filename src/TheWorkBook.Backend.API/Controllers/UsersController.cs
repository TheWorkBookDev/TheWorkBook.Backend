using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TheWorkBook.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // GET api/listings
        [HttpGet]
        public IEnumerable<string> GetMyInfo()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
