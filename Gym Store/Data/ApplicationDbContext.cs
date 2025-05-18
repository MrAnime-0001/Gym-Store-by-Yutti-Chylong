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
                new Category { Id = 3, Name = "Pre-Workout" }
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
                }
            );
        }
    }
}
