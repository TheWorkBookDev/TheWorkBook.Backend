using Microsoft.AspNetCore.JsonPatch;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface ISearchService
    {
        Task<SearchResponse> SearchListings(SearchRequest searchRequest);
    }
}
