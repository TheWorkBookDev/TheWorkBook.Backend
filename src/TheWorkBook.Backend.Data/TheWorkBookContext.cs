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
