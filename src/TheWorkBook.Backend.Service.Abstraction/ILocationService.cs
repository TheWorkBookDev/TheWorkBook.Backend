using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetAllAsync();
        Task<List<LocationDto>> GetCountiesAsync();
        Task<LocationDto> GetLocationAsync(int id);
    }
}
