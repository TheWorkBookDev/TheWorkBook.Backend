namespace TheWorkBook.Shared.Dto
{
    public class NewListingDto
    {
        public decimal Budget { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public string MainDescription { get; set; }
        public string Telephone { get; set; }
        public string Title { get; set; }
    }
}
