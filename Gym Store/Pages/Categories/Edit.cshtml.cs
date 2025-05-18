using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gym_Store.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public Category Category { get; set; }

        public EditModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult OnGet(int id)
        {
            Category = _dbContext.Categories.Find(id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryFromDb = _dbContext.Categories.Find(Category.Id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            categoryFromDb.Name = Category.Name;

            _dbContext.SaveChanges();
            TempData["success"] = "Category updated successfully!";
            return RedirectToPage("Index");
        }
    }
}
