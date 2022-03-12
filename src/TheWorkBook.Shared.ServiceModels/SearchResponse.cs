using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Shared.ServiceModels
{
    public class SearchResponse
    {
        public List<ListingDto> Listings { get; set; }
    }
}
