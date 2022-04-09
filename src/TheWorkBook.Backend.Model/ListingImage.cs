using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheWorkBook.Backend.Model
{
    [Table("ListingImage")]
    public partial class ListingImage
    {
        [Key]
        public int ListingImageId { get; set; }
        public int ListingId { get; set; }
        [Required]
        [StringLength(200)]
        public string ImageUrl { get; set; }
        public byte StatusId { get; set; }
        [Precision(0)]
        public DateTime RecordCreatedUtc { get; set; }
        [Precision(0)]
        public DateTime RecordUpdatedUtc { get; set; }

        [ForeignKey(nameof(ListingId))]
        [InverseProperty("ListingImages")]
        public virtual Listing Listing { get; set; }
    }
}
