using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheWorkBook.Backend.Model
{
    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            InverseParentLocation = new HashSet<Location>();
            Listings = new HashSet<Listing>();
        }

        [Key]
        public int LocationId { get; set; }
        public int? ParentLocationId { get; set; }
        [Required]
        [StringLength(50)]
        public string LocationName { get; set; }
        public byte LocationTypeId { get; set; }
        [Precision(0)]
        public DateTime? RecordCreatedUtc { get; set; }

        [ForeignKey(nameof(ParentLocationId))]
        [InverseProperty(nameof(Location.InverseParentLocation))]
        public virtual Location ParentLocation { get; set; }
        [InverseProperty(nameof(Location.ParentLocation))]
        public virtual ICollection<Location> InverseParentLocation { get; set; }
        [InverseProperty(nameof(Listing.Location))]
        public virtual ICollection<Listing> Listings { get; set; }
    }
}
