using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Gym_Store.Pages.Products
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Product Product { get; set; }

        public DeleteModel(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Load the product to confirm deletion
        public IActionResult OnGet(int id)
        {
            Product = _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        // POST: Delete the product
        public IActionResult OnPost()
        {
            var productInDb = _dbContext.Products.Find(Product.Id);
            if (productInDb == null)
            {
                return NotFound();
            }

            // Delete image file from wwwroot if it exists
            if (!string.IsNullOrEmpty(productInDb.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _dbContext.Products.Remove(productInDb);
            _dbContext.SaveChanges();

            TempData["success"] = "Product deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
