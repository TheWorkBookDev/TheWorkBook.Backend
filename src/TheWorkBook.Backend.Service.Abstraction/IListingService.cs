using Microsoft.AspNetCore.JsonPatch;
using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface IListingService
    {
        Task AddListingAsync(int userId, NewListingDto listingDto);
        Task DeactivateListing(int listingId);
        Task<ListingDto> GetListingAsync(int id);
        Task<IEnumerable<ListingDto>> GetMyListingsAsync();
        Task UpdateListingAsync(int listingId, JsonPatchDocument<ListingDto> patchDocCateogryDto);

        Task UpdateListingAsync(ListingDto listingDto);
    }
}
