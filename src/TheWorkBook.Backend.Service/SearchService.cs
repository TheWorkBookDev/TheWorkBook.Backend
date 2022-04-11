using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Model;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.Service
{
    public class SearchService : BaseService, ISearchService
    {
        public SearchService(IMapper mapper, ILogger<BaseService> logger,
            IApplicationUser applicationUser,
            IEnvVariableHelper envVariableHelper,
            TheWorkBookContext theWorkBookContext
        )
            : base(mapper, logger, applicationUser, envVariableHelper, theWorkBookContext) { }

        public async Task<SearchResponse> SearchListings(SearchRequest searchRequest)
        {
            IQueryable<Listing> listingsQuery = TheWorkBookContext.Listings
                .AsNoTracking().Include(l => l.Location)
                .Include(l => l.Category)
                .Include(l => l.ListingComments)
                .Include(l => l.ListingComments)
                .Include(l => l.ListingImages)
                .AsQueryable();

            if (searchRequest.Categories != null && searchRequest.Categories.Any())
            {
                // Filter by category
                listingsQuery = listingsQuery.Where(l => searchRequest.Categories.Contains(l.CategoryId));
            }

            if (searchRequest.Locations != null && searchRequest.Locations.Any())
            {
                // Filter by location
                listingsQuery = listingsQuery.Where(l => searchRequest.Locations.Contains(l.LocationId));
            }

            // We should allow the user to choose the sort order they want. For now, we will show newest first.
            listingsQuery = listingsQuery.OrderByDescending(l => l.RecordCreatedUtc);

            List<Listing> listings = await listingsQuery.ToListAsync();
            List<ListingDto> listingDtos = Mapper.Map<List<ListingDto>>(listings);

            SearchResponse searchResponse = new SearchResponse
            {
                Listings = listingDtos
            };

            return searchResponse;
        }
    }
}
