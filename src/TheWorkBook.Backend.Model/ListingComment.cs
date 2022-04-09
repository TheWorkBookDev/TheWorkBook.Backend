using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheWorkBook.Backend.Model
{
    [Table("ListingComment")]
    public partial class ListingComment
    {
        [Key]
        public int ListingCommentId { get; set; }
        public int ListingId { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(2000)]
        public string Comment { get; set; }
        [Precision(0)]
        public DateTime RecordCreatedUtc { get; set; }
        [Precision(0)]
        public DateTime RecordUpdatedUtc { get; set; }

        [ForeignKey(nameof(ListingId))]
        [InverseProperty("ListingComments")]
        public virtual Listing Listing { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("ListingComments")]
        public virtual User User { get; set; }
    }
}
