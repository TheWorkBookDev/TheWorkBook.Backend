using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.Service
{
    public class BaseService
    {
        protected readonly IMapper Mapper;
        protected readonly ILogger<BaseService> Logger;
        //protected readonly IApplicationUser ApplicationUser;
        protected readonly IEnvVariableHelper EnvVariableHelper;

        public BaseService(IMapper mapper, ILogger<BaseService> logger,
           //IApplicationUser applicationUser,
           IEnvVariableHelper envVariableHelper)
        {
            Mapper = mapper;
            Logger = logger;
            //ApplicationUser = applicationUser;
            EnvVariableHelper = envVariableHelper;
        }
    }
}
