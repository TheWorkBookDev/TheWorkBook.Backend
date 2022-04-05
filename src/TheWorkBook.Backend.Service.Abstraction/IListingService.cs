using Microsoft.AspNetCore.JsonPatch;
using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface IListingService
    {
        Task AddListingAsync(ListingDto listingDto);
        Task<ListingDto> GetListingAsync(int id);
        Task UpdateListingAsync(int listingId, JsonPatchDocument<ListingDto> patchDocCateogryDto);
    }
}
