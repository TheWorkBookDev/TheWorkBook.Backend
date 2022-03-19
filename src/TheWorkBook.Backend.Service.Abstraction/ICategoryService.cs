using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategories(int? parentCategoryId = null);
    }
}
