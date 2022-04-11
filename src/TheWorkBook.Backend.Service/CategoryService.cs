using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Model;
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


        public async Task AddCategoryAsync(CategoryDto category)
        {
            var categoryEntity = Mapper.Map<Model.Category>(category);
            await TheWorkBookContext.Categories.AddAsync(categoryEntity);
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(int? parentCategoryId = null)
        {
            IQueryable<Model.Category> categoryQuery = TheWorkBookContext.Categories.AsNoTracking().AsQueryable();

            if (parentCategoryId.HasValue)
            {
                categoryQuery = categoryQuery.Where(cat => cat.ParentCategoryId == parentCategoryId);
            }

            categoryQuery = categoryQuery.OrderBy(cat => cat.SortOrder)
                .ThenBy(cat => cat.CategoryName);

            List<Category> categories = await categoryQuery.ToListAsync();

            List<CategoryDto> categoryDtos = Mapper.Map<List<CategoryDto>>(categories);

            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategoryAsync(int categoryId)
        {
            Model.Category category = await TheWorkBookContext.Categories.FindAsync(categoryId);
            CategoryDto categoryDto = Mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task UpdateCategoryAsync(int categoryId, JsonPatchDocument<CategoryDto> patchDocCateogryDto)
        {
            JsonPatchDocument<Category> patchDocument = Mapper.Map<JsonPatchDocument<Category>>(patchDocCateogryDto);

            // We need to identify what fields in the CategoryDto object cannot be updated here.
            var uneditablePaths = new List<string> { "/RecordCreatedUtc" };

            if (patchDocument.Operations.Any(operation => uneditablePaths.Contains(operation.path)))
            {
                throw new UnauthorizedAccessException();
            }

            Model.Category category = await TheWorkBookContext.Categories.FindAsync(categoryId);
            patchDocument.ApplyTo(category);
            await TheWorkBookContext.SaveChangesAsync();
        }
    }
}
