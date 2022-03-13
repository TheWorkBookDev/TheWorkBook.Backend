using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Service.Abstraction;
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
    }
}
