using Microsoft.EntityFrameworkCore;

namespace ubereats.Models
{
    public class RestaurantContext: DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; } = null!;

        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().HasData(
                    new Restaurant {
                        ID = 1,
                        RestName = "DefaultRestName",
                        District = "DefaultDistrict",
                        KitchenType = "DefaultKitchenType",
                        CookingTime = DateTime.Now.TimeOfDay,
                        Image = convertImageToByte(@"C:\dev\UberEats\wwwroot\DefaultImage.jpg"),
                        isDeleted = true
                    }
            );
        }

        private byte[] convertImageToByte(string path)
        {
            byte[] image;
            image = File.ReadAllBytes(path);
            return image;
        }
    }
}
