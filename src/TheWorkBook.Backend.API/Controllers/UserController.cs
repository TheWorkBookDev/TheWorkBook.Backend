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
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/user/[action]")]
    [Produces("application/json")]
    public class UserController : BaseController
    {
        public UserController(ILogger<UserController> logger,
            IEnvVariableHelper envVariableHelper)
            : base(logger, envVariableHelper)
        {

        }

        //[Authorize(Policy = "ext.user.api.policy")]
        [HttpGet]
        [ActionName("getMyInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyInfo()
        {
            UserDto user = new UserDto
            {
                FirstName = "Ronan",
                LastName = "Farrell",
                Email = "ronanfarrell@live.ie",
                Mobile = "083-4508108"
            };
            return Ok(user);
        }

        //[Authorize(Policy = "ext.user.api.policy")]
        [HttpPatch]
        [ActionName("updateMyInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMyUser([FromBody] JsonPatchDocument<UserDto> patchDocUserDto)
        {
            if (patchDocUserDto == null) return BadRequest();

            //int userKey = ApplicationUser.UserKey.Value;
            //await userService.UpdateUserAsync(userKey, patchDocUserDto);
            return Ok();
        }
    }
}
