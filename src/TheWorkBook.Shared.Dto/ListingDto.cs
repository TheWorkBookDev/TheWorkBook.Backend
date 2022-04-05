using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorkBook.Shared.Dto
{
    public class ListingDto
    {
        public int ListingId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string MainDescription { get; set; }
        public decimal Budget { get; set; }
        public byte StatusId { get; set; }
        public DateTime RecordCreatedUtc { get; set; }
        public DateTime RecordUpdatedUtc { get; set; }
        public virtual CategoryDto Category { get; set; }
        public virtual LocationDto Location { get; set; }
    }
}
