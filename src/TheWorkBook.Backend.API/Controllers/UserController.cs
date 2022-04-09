using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/user/[action]")]
    [Produces("application/json")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IApplicationUser _applicationUser;

        public UserController(ILogger<UserController> logger,
            IEnvVariableHelper envVariableHelper,
            IUserService userService,
            IApplicationUser applicationUser)
            : base(logger, envVariableHelper)
        {
            _userService = userService;
            _applicationUser = applicationUser;
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpGet]
        [ActionName("getMyInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyInfo()
        {
            UserDto user = await _userService.GetUser(_applicationUser.UserId.Value);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest createUserRequest)
        {
            if (createUserRequest == null) return BadRequest();

            await _userService.RegisterUser(createUserRequest);
            return Created("", "");
        }

        [Authorize(Policy = "ext.user.api.policy")]
        [HttpPatch]
        [ActionName("updateMyInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMyInfo([FromBody] JsonPatchDocument<UserDto> patchDocUserDto)
        {
            if (patchDocUserDto == null) return BadRequest();

            await _userService.UpdateUserAsync(_applicationUser.UserId.Value, patchDocUserDto);

            return Ok();
        }
    }
}
