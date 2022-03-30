using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface IListingService
    {
        Task AddListingAsync(ListingDto listingDto);
        Task<ListingDto> GetListingAsync(int id);
    }
}
