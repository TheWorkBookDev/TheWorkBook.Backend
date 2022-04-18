namespace TheWorkBook.Shared.Dto
{
    public class ListingDto
    {
        public decimal Budget { get; set; }
        public CategoryDto Category { get; set; }
        public int CategoryId { get; set; }
        public List<ListingCommentDto> Comments { get; set; }
        public int ListingId { get; set; }
        public List<ListingImageDto> ListingImages { get; set; }
        public LocationDto Location { get; set; }
        public int LocationId { get; set; }
        public string MainDescription { get; set; }
        public DateTime RecordCreatedUtc { get; set; }
        public DateTime RecordUpdatedUtc { get; set; }
        public byte StatusId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
    }
}
