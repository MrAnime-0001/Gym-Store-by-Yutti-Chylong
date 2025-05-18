using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gym_Store.Pages.Products
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public List<SelectListItem> CategoryList { get; set; }


        public Product Product { get; set; }

        [BindProperty]
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
            {
                return NotFound();
            }

            CategoryList = _dbContext.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CategoryList = _dbContext.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();

                return Page();
            }

            var productFromDb = _dbContext.Products.Find(Product.Id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            if (ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Gym_Store", "Images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                productFromDb.ImageUrl = $"/Gym_Store/Images/{fileName}";
            }

            productFromDb.Name = Product.Name;
            productFromDb.Price = Product.Price;
            productFromDb.Quantity = Product.Quantity;
            productFromDb.CategoryId = Product.CategoryId;

            _dbContext.SaveChanges();
            TempData["success"] = "Product updated successfully!";
            return RedirectToPage("Index");
        }
    }
}
