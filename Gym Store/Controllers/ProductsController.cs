using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // Index Action: List all products
        public IActionResult Index()
        {
            var products = _dbContext.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // Create Action: Display the create form
        public IActionResult Create()
        {
            var categoryList = _dbContext.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();

            ViewData["CategoryList"] = categoryList; // Use ViewData here
            return View(new Product());
        }

        // Create Action: Handle form submission to create a new product
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            if (product.CategoryId == 0)
            {
                ModelState.AddModelError("Product.CategoryId", "Please select a category.");
            }

            if (!ModelState.IsValid)
            {
                var categoryList = _dbContext.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();

                ViewData["CategoryList"] = categoryList; // Use ViewData here
                return View(product);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Gym_Store", "Images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImageUrl = $"/Gym_Store/Images/{fileName}";
            }

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Product created successfully!";
            return RedirectToAction("Index");
        }

        // Edit Action: Display the edit form for a product
        public IActionResult Edit(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var categoryList = _dbContext.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();

            ViewData["CategoryList"] = categoryList; // Use ViewData here
            return View(product);
        }

        // Edit Action: Handle form submission to update the product
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var categoryList = _dbContext.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();

                ViewData["CategoryList"] = categoryList; // Use ViewData here
                return View(product);
            }

            var productFromDb = _dbContext.Products.Find(product.Id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            // Update image if new file is uploaded
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Gym_Store", "Images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                productFromDb.ImageUrl = $"/Gym_Store/Images/{fileName}";
            }

            // Update other fields
            productFromDb.Name = product.Name;
            productFromDb.Price = product.Price;
            productFromDb.Quantity = product.Quantity;
            productFromDb.ServingSize = product.ServingSize;
            productFromDb.CategoryId = product.CategoryId;

            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }

        // Delete Action: Display confirmation page to delete a product
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Delete Action: Handle deletion of the product
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete image if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
