using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gym_Store.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Load the category to confirm deletion
        public IActionResult OnGet(int id)
        {
            Category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        // POST: Delete the category
        public IActionResult OnPost()
        {
            var categoryInDb = _dbContext.Categories.Find(Category.Id);
            if (categoryInDb == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(categoryInDb);
            _dbContext.SaveChanges();

            TempData["success"] = "Category deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
