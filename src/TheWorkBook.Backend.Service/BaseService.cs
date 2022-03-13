using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorkBook.Utils.Abstraction;
using TheWorkBook.Backend.Data;
using TheWorkBook.AspNetCore.IdentityModel;

namespace TheWorkBook.Backend.Service
{
    public class BaseService
    {
        protected readonly IMapper Mapper;
        protected readonly ILogger<BaseService> Logger;
        protected readonly IApplicationUser ApplicationUser;
        protected readonly IEnvVariableHelper EnvVariableHelper;
        protected readonly TheWorkBookContext TheWorkBookContext;

        public BaseService(IMapper mapper, ILogger<BaseService> logger,
           IApplicationUser applicationUser,
           IEnvVariableHelper envVariableHelper,
           TheWorkBookContext theWorkBookContext)
        {
            Mapper = mapper;
            Logger = logger;
            ApplicationUser = applicationUser;
            EnvVariableHelper = envVariableHelper;
            TheWorkBookContext = theWorkBookContext;
        }
    }
}
