using System.Linq;
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
    public class LocationService : BaseService, ILocationService
    {
        public LocationService(IMapper mapper, ILogger<BaseService> logger,
           IApplicationUser applicationUser,
           IEnvVariableHelper envVariableHelper,
            TheWorkBookContext theWorkBookContext
        )
           : base(mapper, logger, applicationUser, envVariableHelper, theWorkBookContext) { }

        public async Task<List<LocationDto>> GetAllAsync()
        {
            List<Location> locations = await TheWorkBookContext.Locations.ToListAsync();
            return Mapper.Map<List<LocationDto>>(locations);
        }

        public async Task<List<LocationDto>> GetCountiesAsync()
        {
            List<Location> locations = await TheWorkBookContext.Locations.AsNoTracking()
                .Where(loc => loc.LocationTypeId == 2)
                .OrderBy(loc=>loc.LocationName)
                .ToListAsync();
            return Mapper.Map<List<LocationDto>>(locations);
        }

        public async Task<LocationDto> GetLocationAsync(int id)
        {
            Location location = await TheWorkBookContext.Locations.FindAsync(id);
            return Mapper.Map<LocationDto>(location);
        }
    }
}
