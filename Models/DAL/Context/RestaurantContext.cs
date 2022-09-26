using Microsoft.EntityFrameworkCore;
using ubereats.Models;

namespace ubereats.DAL.Context
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().HasData(
                    new Restaurant
                    {
                        ID = 1,
                        RestName = "DefaultRestName",
                        District = "DefaultDistrict",
                        KitchenType = "DefaultKitchenType",
                        CookingTimeStart = 0,
                        CookingTimeEnd = 5,
                        Image = convertImageToByte(@"\wwwroot\DefaultImage.jpg")
                    },
                    new Restaurant
                    {
                        ID = 2,
                        RestName = "Макдоналдс",
                        District = "Газетный",
                        KitchenType = "₽₽ • Бургеры",
                        CookingTimeStart = 25,
                        CookingTimeEnd = 35,
                        Image = convertImageToByte(@"\wwwroot\mc.png")
                    },
                    new Restaurant
                    {
                        ID = 3,
                        RestName = "DimSum & Co",
                        District = "ЦДМ",
                        KitchenType = "₽ • Японская • Китайская • Азиатская",
                        CookingTimeStart = 40,
                        CookingTimeEnd = 50,
                        Image = convertImageToByte(@"\wwwroot\DimSum.png")
                    },
                    new Restaurant
                    {
                        ID = 4,
                        RestName = "ДвижОК",
                        District = "Манежная",
                        KitchenType = "₽ • Американская • Европейская",
                        CookingTimeStart = 35,
                        CookingTimeEnd = 45,
                        Image = convertImageToByte(@"\wwwroot\ДвижОК.png")
                    },
                    new Restaurant
                    {
                        ID = 5,
                        RestName = "НЯ",
                        District = "NHA",
                        KitchenType = "₽₽ • Вьетнамская",
                        CookingTimeStart = 30,
                        CookingTimeEnd = 40,
                        Image = convertImageToByte(@"\wwwroot\НЯ.png")
                    },
                    new Restaurant
                    {
                        ID = 6,
                        RestName = "Точка Дзы",
                        District = "Цветной",
                        KitchenType = "₽₽ • Вьетнамская",
                        CookingTimeStart = 40,
                        CookingTimeEnd = 50,
                        Image = convertImageToByte(@"\wwwroot\Дзы.png")
                    },
                    new Restaurant
                    {
                        ID = 7,
                        RestName = "Cinnabon",
                        District = string.Empty,
                        KitchenType = "₽ • Выпечка • Десерты • Капкейки",
                        CookingTimeStart = 25,
                        CookingTimeEnd = 35,
                        Image = convertImageToByte(@"\wwwroot\Cinnabon.png")
                    },
                    new Restaurant
                    {
                        ID = 8,
                        RestName = "PIZZELOVE",
                        District = string.Empty,
                        KitchenType = "₽₽ • Пицца",
                        CookingTimeStart = 15,
                        CookingTimeEnd = 25,
                        Image = convertImageToByte(@"\wwwroot\PIZZELOVE.png")
                    },
                    new Restaurant
                    {
                        ID = 9,
                        RestName = "Zю кафе",
                        District = "Тверская",
                        KitchenType = "₽₽ • Японская",
                        CookingTimeStart = 25,
                        CookingTimeEnd = 35,
                        Image = convertImageToByte(@"\wwwroot\Zю.png")
                    },
                    new Restaurant
                    {
                        ID = 10,
                        RestName = "Bar BQ Cafe",
                        District = "Манежная",
                        KitchenType = "₽₽₽ • Европейская",
                        CookingTimeStart = 30,
                        CookingTimeEnd = 40,
                        Image = convertImageToByte(@"\wwwroot\BQ.png")
                    }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = 1,
                    loginname = "admin",
                    password = "admin",
                    email = "admin@mail.ru",
                });
        }

        private byte[] convertImageToByte(string path)
        {
            byte[] image;
            image = File.ReadAllBytes(Environment.CurrentDirectory + path);
            return image;
        }
    }
}
