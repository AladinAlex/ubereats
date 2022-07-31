using Microsoft.EntityFrameworkCore;

namespace ubereats.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Restaurants { get; set; } = null!;

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = 1,
                    loginname = "admin",
                    password = "admin",
                    email = "admin@mail.ru"
                }
            );
        }
    }
}
