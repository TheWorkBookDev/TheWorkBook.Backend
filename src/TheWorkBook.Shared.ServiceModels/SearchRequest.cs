using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorkBook.Shared.ServiceModels
{
    public class SearchRequest
    {
        public List<int> Categories { get; set; }
        public List<int> Locations { get; set; }
    }
}
