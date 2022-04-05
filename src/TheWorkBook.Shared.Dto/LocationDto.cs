namespace TheWorkBook.Shared.Dto
{
    public class LocationDto
    {
        public int LocationId { get; set; }
        public int? ParentLocationId { get; set; }
        public string LocationName { get; set; }
        public byte LocationTypeId { get; set; }
    }
}
