using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheWorkBook.Backend.Model
{
    [Table("Category")]
    public partial class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        [Required]
        public bool? DisplayOnMainNav { get; set; }
        public bool HasChildCategory { get; set; }
        [Precision(0)]
        public DateTime RecordCreatedUtc { get; set; }
        public DateTime RecordUpdatedUtc { get; set; }
    }
}
