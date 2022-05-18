using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TheWorkBook.Backend.Model;

namespace TheWorkBook.Backend.Data
{
    public partial class TheWorkBookContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Listing> Listings { get; set; }
        public virtual DbSet<ListingComment> ListingComments { get; set; }
        public virtual DbSet<ListingImage> ListingImages { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public TheWorkBookContext(DbContextOptions<TheWorkBookContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.DisplayOnMainNav).HasDefaultValueSql("((1))");

                entity.Property(e => e.RecordCreatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RecordUpdatedUtc).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.SortOrder).HasDefaultValueSql("((10))");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Listing>(entity =>
            {
                entity.Property(e => e.Budget).HasDefaultValueSql("((0.00))");

                entity.Property(e => e.RecordCreatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RecordUpdatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Listing_Category");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Listing_Location");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Listing_User");
            });

            modelBuilder.Entity<ListingComment>(entity =>
            {
                entity.Property(e => e.ListingCommentId).ValueGeneratedNever();

                entity.Property(e => e.RecordCreatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RecordUpdatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.ListingComments)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListingComment_Listing");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ListingComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListingComment_User");
            });

            modelBuilder.Entity<ListingImage>(entity =>
            {
                entity.Property(e => e.RecordCreatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RecordUpdatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.ListingImages)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListingImage_Listing");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.RecordCreatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.ParentLocation)
                    .WithMany(p => p.InverseParentLocation)
                    .HasForeignKey(d => d.ParentLocationId)
                    .HasConstraintName("FK_Location_Location");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.RecordCreatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RecordUpdatedUtc)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(sysutcdatetime())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
