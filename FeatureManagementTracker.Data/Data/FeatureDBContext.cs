using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FeatureManagementTracker.Data
{
    public class FeatureDBContext : DbContext
    {
        public FeatureDBContext(DbContextOptions<FeatureDBContext> options)
            : base(options)
        {
        }
        public DbSet<Feature> Features { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feature>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Feature>()
                .HasIndex(f => f.Title)
                .HasDatabaseName("IX_Feature_Title");

            modelBuilder.Entity<Feature>()
                .HasIndex(f => f.Status)
                .HasDatabaseName("IX_Feature_Status");
        }
    }
}
