using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Gym_Store.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public Dictionary<string, List<Product>> ProductsByCategory { get; set; }

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            var productsWithCategory = _dbContext.Products
                .Include(p => p.Category) // Include category navigation property
                .ToList();

            ProductsByCategory = productsWithCategory
                .GroupBy(p => p.Category?.Name ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
