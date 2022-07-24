﻿using Microsoft.EntityFrameworkCore;

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
                        CookingTimeStart = 0,
                        CookingTimeEnd = 5,
                        Image = convertImageToByte(@"C:\dev\UberEats\wwwroot\DefaultImage.jpg"),
                        isDeleted = true
                    }
                    //,
                    //new Restaurant {
                    //    ID = 2,
                    //    RestName = "Макдоналдс",
                    //    District = "Газетный",
                    //    KitchenType = "₽₽ • Бургеры",
                    //    CookingTimeStart = 25,
                    //    CookingTimeEnd = 35,
                    //    Image = convertImageToByte(@"C:\dev\UberEats\wwwroot\mc.png"), isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 3,
                    //    RestName = "DimSum & Co",
                    //    District = "ЦДМ",
                    //    KitchenType = "₽ • Японская • Китайская • Азиатская",
                    //    CookingTimeStart = 40,
                    //    CookingTimeEnd = 50,
                    //    Image = convertImageToByte(@"C:\dev\test\DimSum.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 4,
                    //    RestName = "ДвижОК",
                    //    District = "Манежная",
                    //    KitchenType = "₽ • Американская • Европейская",
                    //    CookingTimeStart = 35,
                    //    CookingTimeEnd = 45,
                    //    Image = convertImageToByte(@"C:\dev\test\ДвижОК.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 5,
                    //    RestName = "НЯ",
                    //    District = "NHA",
                    //    KitchenType = "₽₽ • Вьетнамская",
                    //    CookingTimeStart = 30,
                    //    CookingTimeEnd = 40,
                    //    Image = convertImageToByte(@"C:\dev\test\НЯ.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 6,
                    //    RestName = "Точка Дзы",
                    //    District = "Цветной",
                    //    KitchenType = "₽₽ • Вьетнамская",
                    //    CookingTimeStart = 40,
                    //    CookingTimeEnd = 50,
                    //    Image = convertImageToByte(@"C:\dev\test\Дзы.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 7,
                    //    RestName = "Cinnabon",
                    //    District = String.Empty,
                    //    KitchenType = "₽ • Выпечка • Десерты • Капкейки",
                    //    CookingTimeStart = 25,
                    //    CookingTimeEnd = 35,
                    //    Image = convertImageToByte(@"C:\dev\test\Cinnabon.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 8,
                    //    RestName = "PIZZELOVE",
                    //    District = String.Empty,
                    //    KitchenType = "₽₽ • Пицца",
                    //    CookingTimeStart = 15,
                    //    CookingTimeEnd = 25,
                    //    Image = convertImageToByte(@"C:\dev\test\PIZZELOVE.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    RestName = "Zю кафе",
                    //    District = "Тверская",
                    //    KitchenType = "₽₽ • Японская",
                    //    CookingTimeStart = 25,
                    //    CookingTimeEnd = 35,
                    //    Image = convertImageToByte(@"C:\dev\test\Zю.png"),
                    //    isDeleted = false
                    //},
                    //new Restaurant {
                    //    ID = 10,
                    //    RestName = "Bar BQ Cafe",
                    //    District = "Манежная",
                    //    KitchenType = "₽₽₽ • Европейская",
                    //    CookingTimeStart = 30,
                    //    CookingTimeEnd = 40,
                    //    Image = convertImageToByte(@"C:\dev\test\BQ.png"),
                    //    isDeleted = false
                    //}
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
