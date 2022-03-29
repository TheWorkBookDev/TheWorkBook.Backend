using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.Service
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IMapper mapper, ILogger<BaseService> logger,
            IApplicationUser applicationUser,
            IEnvVariableHelper envVariableHelper,
            TheWorkBookContext theWorkBookContext
        )
            : base(mapper, logger, applicationUser, envVariableHelper, theWorkBookContext) { }

        public async Task<List<CategoryDto>> GetCategories(int? parentCategoryId = null)
        {
            IQueryable<Model.Category> categoryQuery = TheWorkBookContext.Categories.AsNoTracking().AsQueryable();

            if (parentCategoryId.HasValue)
            {
                categoryQuery = categoryQuery.Where(cat=>cat.ParentCategoryId == parentCategoryId);
            }

            // TODO: Add SortOrder column
            categoryQuery = categoryQuery.OrderBy(cat => cat.CategoryName);

            List<Model.Category> categories = await categoryQuery.ToListAsync();

            List<CategoryDto> categoryDtos = Mapper.Map<List<CategoryDto>>(categories);

            return categoryDtos;
        }
    }
}
