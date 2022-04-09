namespace TheWorkBook.Shared.Dto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? DisplayOnMainNav { get; set; }
        public bool HasChildCategory { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
