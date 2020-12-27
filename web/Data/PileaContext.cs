using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace web.Data
{

    // The main class that coordinates Entity Framework functionality for a given data model 
    // is the database context class.
    public class PileaContext : IdentityDbContext<User>
    {
        public PileaContext(DbContextOptions<PileaContext> options) : base(options)
        {
        }

        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Category> Categories { get; set; }
        // public DbSet<LocalUser> LocalUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); 

            // Override and change database table names (singularity)
            modelBuilder.Entity<Friendship>().ToTable("Friendship");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Plant>().ToTable("Plant");
            modelBuilder.Entity<Category>().ToTable("Type");

           
            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasKey(x => new { x.UserId, x.UserFriendId });

                b.HasOne(x => x.User)
                    .WithMany(x => x.Friends)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.UserFriend)
                    .WithMany(x => x.FriendsOf)
                    .HasForeignKey(x => x.UserFriendId)
                    .OnDelete(DeleteBehavior.Restrict);
            });            


        }
        public override  DbSet<User> Users { get; set; }
    }
}