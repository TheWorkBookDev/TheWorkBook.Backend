using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Model;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.Service
{
    public class ListingService : BaseService, IListingService
    {
        public ListingService(IMapper mapper, ILogger<BaseService> logger,
            IApplicationUser applicationUser,
            IEnvVariableHelper envVariableHelper,
            TheWorkBookContext theWorkBookContext
        )
            : base(mapper, logger, applicationUser, envVariableHelper, theWorkBookContext) { }

        public async Task AddListingAsync(ListingDto listingDto)
        {
            Listing listing = Mapper.Map<Listing>(listingDto);
            TheWorkBookContext.Listings.Add(listing);
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task<ListingDto> GetListingAsync(int id)
        {
            Listing listing = await TheWorkBookContext.Listings.FindAsync(id);
            return Mapper.Map<ListingDto>(listing);
        }
    }
}
