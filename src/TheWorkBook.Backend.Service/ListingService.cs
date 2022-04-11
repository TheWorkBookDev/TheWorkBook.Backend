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
            IQueryable<Listing> listingQuery = GetListingQuery();
            Listing listing = await listingQuery.FirstOrDefaultAsync(l => l.ListingId == id);
            return Mapper.Map<ListingDto>(listing);
        }

        public async Task UpdateListingAsync(int listingId, JsonPatchDocument<ListingDto> patchDocCateogryDto)
        {
            JsonPatchDocument<Listing> patchDocument = Mapper.Map<JsonPatchDocument<Listing>>(patchDocCateogryDto);

            // We need to identify what fields in the UserDto object cannot be updated here.
            var uneditablePaths = new List<string> { "/RecordCreatedUtc" };

            if (patchDocument.Operations.Any(operation => uneditablePaths.Contains(operation.path)))
            {
                throw new UnauthorizedAccessException();
            }

            Model.Listing category = await TheWorkBookContext.Listings.FindAsync(listingId);
            patchDocument.ApplyTo(category);
            await TheWorkBookContext.SaveChangesAsync();
        }

        private IQueryable<Listing> GetListingQuery()
        {
            IQueryable<Listing> listingQuery = TheWorkBookContext.Listings
               .AsNoTracking().Include(l => l.Location)
               .Include(l => l.Category)
               .Include(l => l.ListingComments)
               .Include(l => l.ListingComments)
               .Include(l => l.ListingImages)
               .AsQueryable();
            return listingQuery;
        }
    }
}
