using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        // POST: Delete the product
        public async Task<IActionResult> OnPostAsync()
        {
            var productInDb = await _dbContext.Products.FindAsync(Product.Id);
            if (productInDb == null)
            {
                return NotFound();
            }

            // Delete image file from wwwroot if it exists
            if (!string.IsNullOrEmpty(productInDb.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _dbContext.Products.Remove(productInDb);
            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Product deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
