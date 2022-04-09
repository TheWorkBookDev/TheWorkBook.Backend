using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheWorkBook.Backend.Model
{
    [Table("Listing")]
    public partial class Listing
    {
        public Listing()
        {
            ListingComments = new HashSet<ListingComment>();
            ListingImages = new HashSet<ListingImage>();
        }

        [Key]
        public int ListingId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        [Required]
        [StringLength(2000)]
        public string MainDescription { get; set; }
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }
        public byte StatusId { get; set; }
        [Precision(0)]
        public DateTime RecordCreatedUtc { get; set; }
        [Precision(0)]
        public DateTime RecordUpdatedUtc { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Listings")]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Listings")]
        public virtual Location Location { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Listings")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(ListingComment.Listing))]
        public virtual ICollection<ListingComment> ListingComments { get; set; }
        [InverseProperty(nameof(ListingImage.Listing))]
        public virtual ICollection<ListingImage> ListingImages { get; set; }
    }
}
