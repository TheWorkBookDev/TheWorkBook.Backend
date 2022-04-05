using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
            return new SearchResponse();   
        }
    }
}
