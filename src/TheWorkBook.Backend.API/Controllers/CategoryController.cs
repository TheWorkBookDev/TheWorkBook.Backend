using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    [Route("v{version:apiVersion}/category/[action]")]
    public class CategoryController : BaseController
    {
        readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
            IEnvVariableHelper envVariableHelper,
            ICategoryService categoryService)
            : base(logger, envVariableHelper)
        {
            _categoryService = categoryService;
        }

        [Authorize(Policy = "int.api.policy")]
        [HttpPost]
        [ActionName("add")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(CategoryDto categoryInfo)
        {
            await _categoryService.AddCategoryAsync(categoryInfo);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("get")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int categoryId)
        {
            CategoryDto categoryDto = await _categoryService.GetCategoryAsync(categoryId);
            return Ok(categoryDto);
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("getCategories")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories(int? parentCategoryId = null)
        {
            List<CategoryDto> categories = await _categoryService.GetCategoriesAsync(parentCategoryId);
            return Ok(categories);
        }

        [Authorize(Policy = "int.api.policy")]
        [HttpPatch]
        [ActionName("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] JsonPatchDocument<CategoryDto> patchDocListingDto)
        {
            return Ok();
        }
    }
}
