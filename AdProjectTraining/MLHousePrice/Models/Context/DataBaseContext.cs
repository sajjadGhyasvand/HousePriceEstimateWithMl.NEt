using Microsoft.EntityFrameworkCore;
using MLHousePrice.Models.Entities;

namespace MLHousePrice.Models.Context
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Advertisements)
                .WithOne(a => a.Location)
                .HasForeignKey(a => a.LocationId);

           
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "جردن" },
                new Location { Id = 2, Name = "جیحون" }
            );
        }
    }
}
