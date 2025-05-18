using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Store.Pages.Products
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Product Product { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public IFormFile? ImageFile { get; set; }

        public EditModel(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet(int id)
        {
            Product = _dbContext.Products.Find(id);
            if (Product == null)
                return NotFound();

            PopulateCategoryList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategoryList();
                return Page();
            }

            var productFromDb = _dbContext.Products.Find(Product.Id);
            if (productFromDb == null)
                return NotFound();

            // Update image if new file is uploaded
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Gym_Store", "Images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                productFromDb.ImageUrl = $"/Gym_Store/Images/{fileName}";
            }

            // Update other fields
            productFromDb.Name = Product.Name;
            productFromDb.Price = Product.Price;
            productFromDb.Quantity = Product.Quantity;
            productFromDb.ServingSize = Product.ServingSize;
            productFromDb.CategoryId = Product.CategoryId;

            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Product updated successfully!";
            return RedirectToPage("Index");
        }

        private void PopulateCategoryList()
        {
            CategoryList = _dbContext.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
        }
    }
}
