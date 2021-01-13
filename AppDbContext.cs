using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyLandmarks.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Photo>().HasAlternateKey(p => p.Url);
            builder.Entity<Location>().HasAlternateKey(p => p.Name);
            builder.Entity<PhotoLocation>().HasAlternateKey(p => new { p.PhotoId, p.LocationId });
            builder.Entity<PhotoLocation>().HasOne(p => p.Location).WithMany(p => p.Photos).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PhotoLocation>().HasOne(p => p.Photo).WithMany(p => p.Locations).OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PhotoLocation> PhotoLocations { get; set; }
    }
}
