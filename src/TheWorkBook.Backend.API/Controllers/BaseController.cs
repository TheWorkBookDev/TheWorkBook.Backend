using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> Logger;
        protected readonly IEnvVariableHelper EnvVariableHelper;

        public BaseController(ILogger<BaseController> logger,
            IEnvVariableHelper envVariableHelper)
        {
            Logger = logger;
            EnvVariableHelper = envVariableHelper;
        }
    }
}
