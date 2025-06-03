using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var productsWithCategory = await _dbContext.Products
                .Include(p => p.Category)
                .ToListAsync();

            var productsByCategory = productsWithCategory
                .GroupBy(p => p.Category?.Name ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(productsByCategory);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
