using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{

    // The main class that coordinates Entity Framework functionality for a given data model 
    // is the database context class.
    public class PileaContext : IdentityDbContext<ApplicationUser>
    {
        public PileaContext(DbContextOptions<PileaContext> options) : base(options)
        {
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Category> Types { get; set; }
        // public DbSet<LocalUser> LocalUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); 

            // Override and change database table names (singularity)
            modelBuilder.Entity<Friend>().ToTable("Friend");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Plant>().ToTable("Plant");
            modelBuilder.Entity<Category>().ToTable("Type");

        }
    }
}