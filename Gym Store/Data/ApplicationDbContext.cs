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

            // ✅ Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Protein Powder" },
                new Category { Id = 2, Name = "Creatine Supplement" },
                new Category { Id = 3, Name = "Pre-Workout" },
                new Category { Id = 4, Name = "Protein Bars" }
            );

            // ✅ Seed Products with CategoryId instead of Type
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Optimum Nutrition Gold Standard 100% Whey French Vanilla",
                    CategoryId = 1,
                    Price = 98.95m,
                    Quantity = "2.27 kg",
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard 100% Whey.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Myprotein Impact Whey Isolate",
                    CategoryId = 1,
                    Price = 70.50m,
                    Quantity = "1 kg",
                    ImageUrl = "/Gym_Store/Images/Myprotein Impact Whey Isolate.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "INC Creatine Monohydrate",
                    CategoryId = 2,
                    Price = 39.95m,
                    Quantity = "500 g",
                    ImageUrl = "/Gym_Store/Images/INC Creatine Monohydrate.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "EHP Labs Pride Pre-Workout Blue Slushie",
                    CategoryId = 3,
                    Price = 79.95m,
                    Quantity = "40 Serves",
                    ImageUrl = "/Gym_Store/Images/EHP Labs Pride Pre-Workout Blue Slushie.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "EHP Labs Pride Pre-Workout Raspberry Twizzle",
                    CategoryId = 3,
                    Price = 79.95m,
                    Quantity = "40 Serves",
                    ImageUrl = "/Gym_Store/Images/EHP Labs Pride Pre-Workout Raspberry Twizzle.jpg"
                },
                new Product
                {
                    Id = 6,
                    Name = "Optimum Nutrition Gold Standard Pre-Workout Green Apple",
                    CategoryId = 3,
                    Price = 39.90m,
                    Quantity = "30 Serves",
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard Pre-Workout Green Apple.jpg"
                },
                new Product
                {
                    Id = 7,
                    Name = "Optimum Nutrition Gold Standard Pre-Workout Blueberry Lemonade",
                    CategoryId = 3,
                    Price = 39.90m,
                    Quantity = "30 Serves",
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Gold Standard Pre-Workout Blueberry Lemonade.jpg"
                },
                new Product
                {
                    Id = 8,
                    Name = "Musashi Pre-Workout Purple Grape",
                    CategoryId = 3,
                    Price = 29.99m,
                    Quantity = "225 g",
                    ImageUrl = "/Gym_Store/Images/Musashi Pre-Workout Purple Grape.jpg"
                },
                new Product
                {
                    Id = 9,
                    Name = "Optimum Nutrition Micronised Creatine Powder",
                    CategoryId = 2,
                    Price = 39.95m,
                    Quantity = "300 G",
                    ImageUrl = "/Gym_Store/Images/Optimum Nutrition Micronised Creatine Powder.jpg"
                },
                new Product
                {
                    Id = 10,
                    Name = "Musashi 100% Creatine",
                    CategoryId = 2,
                    Price = 32.95m,
                    Quantity = "350 G",
                    ImageUrl = "/Gym_Store/Images/Musashi 100% Creatine.jpg"
                },
                new Product
                {
                    Id = 11,
                    Name = "Musashi Shred & Burn Protein – Vanilla Milkshake",
                    CategoryId = 1,
                    Price = 110.46m,
                    Quantity = "2 kg",
                    ImageUrl = "/Gym_Store/Images/Musashi Shred & Burn Protein – Vanilla Milkshake.jpg"
                },
                new Product
                {
                    Id = 12,
                    Name = "Musashi Shred & Burn Protein – Chocolate Milkshake",
                    CategoryId = 1,
                    Price = 110.46m,
                    Quantity = "2 kg",
                    ImageUrl = "/Gym_Store/Images/Musashi Shred & Burn Protein – Chocolate Milkshake.jpg"
                },
                new Product
                {
                    Id = 13,
                    Name = "Grenade Protein Bar - Oreo",
                    CategoryId = 4,
                    Price = 59.95m,
                    Quantity = "12 x 60 G",
                    ImageUrl = "/Gym_Store/Images/Grenade Protein Bar - Oreo.jpg"
                },
                new Product
                {
                Id = 14,
                    Name = "Chief Collagen Bar  – Double Chocolate",
                    CategoryId = 4,
                    Price = 63.00m,
                    Quantity = "12x 60g",
                    ImageUrl = "/Gym_Store/Images/Chief Collagen Bar  – Double Chocolate.jpg"
                },
                new Product
                {
                    Id = 15,
                    Name = "Grenade Protein Bar - Fudge Up",
                    CategoryId = 4,
                    Price = 59.95m,
                    Quantity = "12 x 60 G",
                    ImageUrl = "/Gym_Store/Images/Grenade Protein Bar - Fudge Up.jpg"
                },
                new Product
                {
                Id = 16,
                    Name = "Horleys Protein 33 Low Carb Bars - Double Chocolate Fudge",
                    CategoryId = 4,
                    Price = 34.95m,
                    Quantity = "12x 60g",
                    ImageUrl = "/Gym_Store/Images/Horleys Protein 33 Low Carb Bars - Double Chocolate Fudge.jpg"
                },
                new Product
                {
                Id = 17,
                Name = "Musashi Protein + Energy Bars - Banana Bread",
                CategoryId = 4,
                Price = 39.96m,
                Quantity = "12x 58g",
                ImageUrl = "/Gym_Store/Images/Musashi Protein + Energy Bars - Banana Bread.jpg"
                }
            );
        }
    }
}
