﻿using AutoMapper;
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

        public async Task AddListingAsync(int userId, NewListingDto listingDto)
        {
            Listing listing = Mapper.Map<Listing>(listingDto);
            listing.UserId = userId;
            listing.StatusId = 1;   // Active
            listing.RecordCreatedUtc = DateTime.UtcNow;
            listing.RecordUpdatedUtc = DateTime.UtcNow;
            TheWorkBookContext.Listings.Add(listing);
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task<ListingDto> GetListingAsync(int id)
        {
            IQueryable<Listing> listingQuery = GetListingQuery();
            Listing listing = await listingQuery.FirstOrDefaultAsync(l => l.ListingId == id);
            return Mapper.Map<ListingDto>(listing);
        }

        public async Task<IEnumerable<ListingDto>> GetMyListingsAsync()
        {
            IQueryable<Listing> listingQuery = GetListingQuery();
            List<Listing> listing = await listingQuery
                .Where(l => l.UserId == ApplicationUser.UserId && l.StatusId > 0)
                .ToListAsync();
            return Mapper.Map<IEnumerable<ListingDto>>(listing);
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

            Model.Listing listing = await TheWorkBookContext.Listings.FindAsync(listingId);
            patchDocument.ApplyTo(listing);
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task UpdateListingAsync(ListingDto listingDto)
        {
            Model.Listing listing = await TheWorkBookContext.Listings.FindAsync(listingDto.ListingId);
            
            listing.Budget = listingDto.Budget;
            listing.CategoryId = listingDto.CategoryId;
            listing.MainDescription = listingDto.MainDescription;
            listing.Title = listingDto.Title;
            listing.Telephone = listingDto.Telephone;
            listing.RecordUpdatedUtc = DateTime.UtcNow;
            listing.StatusId = 1;
           
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task DeactivateListing(int listingId)
        {
            Model.Listing listing = await TheWorkBookContext.Listings.FindAsync(listingId);
            listing.StatusId = 0;
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
