using Gym_Store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gym_Store.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; } // ✅ New DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Protein Powder" },
                new Category { Id = 2, Name = "Creatine Supplement" },
                new Category { Id = 3, Name = "Pre-Workout" },
                new Category { Id = 4, Name = "Protein Bars" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Ghost Whey Protein - Coffee Ice Cream",
                    CategoryId = 1,
                    Price = 89.00m,
                    ServingSize = "64 Serves",
                    Quantity = 6,
                    ImageUrl = "/Gym_Store/Images/Ghost Whey Protein - Coffee Ice Cream.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Ghost Whey Protein - Marshmallow",
                    CategoryId = 1,
                    Price = 89.00m,
                    ServingSize = "64 Serves",
                    Quantity = 22,
                    ImageUrl = "/Gym_Store/Images/Ghost Whey Protein - Marshmallow.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Musashi Shred & Burn Protein – Chocolate Milkshake",
                    CategoryId = 1,
                    Price = 110.46m,
                    ServingSize = "58 Serves",
                    Quantity = 17,
                    ImageUrl = "/Gym_Store/Images/Musashi Shred & Burn Protein – Chocolate Milkshake.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Musashi Shred & Burn Protein – Vanilla Milkshake",
                    CategoryId = 1,
                    Price = 110.46m,
                    ServingSize = "58 Serves",
                    Quantity = 21,
                    ImageUrl = "/Gym_Store/Images/Musashi Shred & Burn Protein – Vanilla Milkshake.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "Myprotein Impact Whey Isolate",
                    CategoryId = 1,
                    Price = 70.50m,
                    ServingSize = "40 Serves",
                    Quantity = 15,
                    ImageUrl = "/Gym_Store/Images/Myprotein Impact Whey Isolate.jpg"
                },
                new Product
                {
                    Id = 6,
                    Name = "Optimum Nutrition Gold Standard 100% Whey - Cookies & Cream",
                    CategoryId = 1,
                    Price = 98.95m,
                    ServingSize = "29 Serves",
                    Quantity = 20,
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard 100% Whey - Cookies & Cream.jpg"
                },
                new Product
                {
                    Id = 7,
                    Name = "Optimum Nutrition Gold Standard 100% Whey - French Vanilla",
                    CategoryId = 1,
                    Price = 98.95m,
                    ServingSize = "29 Serves",
                    Quantity = 18,
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard 100% Whey - French Vanilla.jpg"
                },
                new Product
                {
                    Id = 8,
                    Name = "INC Creatine Monohydrate",
                    CategoryId = 2,
                    Price = 39.95m,
                    ServingSize = "100 Serves",
                    Quantity = 10,
                    ImageUrl = "/Gym_Store/Images/INC Creatine Monohydrate.jpg"
                },
                new Product
                {
                    Id = 9,
                    Name = "Musashi 100% Creatine",
                    CategoryId = 2,
                    Price = 32.95m,
                    ServingSize = "70 Serves",
                    Quantity = 7,
                    ImageUrl = "/Gym_Store/Images/Musashi 100% Creatine.jpg"
                },
                new Product
                {
                    Id = 10,
                    Name = "Optimum Nutrition Micronised Creatine Powder",
                    CategoryId = 2,
                    Price = 39.95m,
                    ServingSize = "60 Serves",
                    Quantity = 20,
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Micronised Creatine Powder.jpg"
                },
                new Product
                {
                    Id = 11,
                    Name = "EHP Labs Pride Pre-Workout Blue Slushie",
                    CategoryId = 3,
                    Price = 79.95m,
                    ServingSize = "40 Serves",
                    Quantity = 14,
                    ImageUrl = "/Gym_Store/Images/EHP Labs Pride Pre-Workout Blue Slushie.jpg"
                },
                new Product
                {
                    Id = 12,
                    Name = "EHP Labs Pride Pre-Workout Raspberry Twizzle",
                    CategoryId = 3,
                    Price = 79.95m,
                    ServingSize = "40 Serves",
                    Quantity = 19,
                    ImageUrl = "/Gym_Store/Images/EHP Labs Pride Pre-Workout Raspberry Twizzle.jpg"
                },
                new Product
                {
                    Id = 13,
                    Name = "Musashi Pre-Workout Purple Grape",
                    CategoryId = 3,
                    Price = 29.99m,
                    ServingSize = "25 Serves",
                    Quantity = 13,
                    ImageUrl = "/Gym_Store/Images/Musashi Pre-Workout Purple Grape.jpg"
                },
                new Product
                {
                    Id = 14,
                    Name = "Optimum Nutrition Gold Standard Pre-Workout Blueberry Lemonade",
                    CategoryId = 3,
                    Price = 39.90m,
                    ServingSize = "30 Serves",
                    Quantity = 16,
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard Pre-Workout Blueberry Lemonade.jpg"
                },
                new Product
                {
                    Id = 15,
                    Name = "Optimum Nutrition Gold Standard Pre-Workout Green Apple",
                    CategoryId = 3,
                    Price = 39.90m,
                    ServingSize = "30 Serves",
                    Quantity = 11,
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard Pre-Workout Green Apple.jpg"
                },
                new Product
                {
                    Id = 16,
                    Name = "Chief Collagen Bar  – Double Chocolate",
                    CategoryId = 4,
                    Price = 63.00m,
                    ServingSize = "12 x 60g",
                    Quantity = 9,
                    ImageUrl = "/Gym_Store/Images/Chief Collagen Bar  – Double Chocolate.jpg"
                },
                new Product
                {
                    Id = 17,
                    Name = "Grenade Protein Bar - Fudge Up",
                    CategoryId = 4,
                    Price = 59.95m,
                    ServingSize = "12 x 60 G",
                    Quantity = 8,
                    ImageUrl = "/Gym_Store/Images/Grenade Protein Bar - Fudge Up.jpg"
                },
                new Product
                {
                    Id = 18,
                    Name = "Grenade Protein Bar - Oreo",
                    CategoryId = 4,
                    Price = 59.95m,
                    ServingSize = "12 x 60 G",
                    Quantity = 22,
                    ImageUrl = "/Gym_Store/Images/Grenade Protein Bar - Oreo.jpg"
                },
                new Product
                {
                    Id = 19,
                    Name = "Horleys Protein 33 Low Carb Bars - Double Chocolate Fudge",
                    CategoryId = 4,
                    Price = 34.95m,
                    ServingSize = "12 x 60g",
                    Quantity = 12,
                    ImageUrl = "/Gym_Store/Images/Horleys Protein 33 Low Carb Bars - Double Chocolate Fudge.jpg"
                },
                new Product
                {
                    Id = 20,
                    Name = "Musashi Protein + Energy Bars - Banana Bread",
                    CategoryId = 4,
                    Price = 39.96m,
                    ServingSize = "12 x 58g",
                    Quantity = 5,
                    ImageUrl = "/Gym_Store/Images/Musashi Protein + Energy Bars - Banana Bread.jpg"
                }
            );
        }

    }
}
