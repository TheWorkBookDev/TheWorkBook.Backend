using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Backend.Service
{
    public class ListingService : BaseService, IListingService
    {
        public ListingService(IMapper mapper, ILogger<BaseService> logger,
           IEnvVariableHelper envVariableHelper)
            : base(mapper, logger, envVariableHelper) { }
    }
}
