using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryDto category);
        Task<List<CategoryDto>> GetCategoriesAsync(int? parentCategoryId = null);
        Task<CategoryDto> GetCategoryAsync(int categoryId);
    }
}
