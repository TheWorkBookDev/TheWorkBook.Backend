using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheWorkBook.Backend.Model
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            ListingComments = new HashSet<ListingComment>();
            Listings = new HashSet<Listing>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(320)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Mobile { get; set; }
        [StringLength(200)]
        public string HashedPassword { get; set; }
        [Precision(0)]
        public DateTime RecordCreatedUtc { get; set; }
        [Precision(0)]
        public DateTime RecordUpdatedUtc { get; set; }

        [InverseProperty(nameof(ListingComment.User))]
        public virtual ICollection<ListingComment> ListingComments { get; set; }
        [InverseProperty(nameof(Listing.User))]
        public virtual ICollection<Listing> Listings { get; set; }
    }
}
