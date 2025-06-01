using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Store.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoriesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Index Action: List all categories
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }

        // Create Action: Display the create form
        public IActionResult Create()
        {
            return View(new Category());
        }

        // Create Action: Handle form submission to create a new category
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Category created successfully!";
            return RedirectToAction("Index");
        }

        // Edit Action: Display the edit form for a category
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Edit Action: Handle form submission to update the category
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var categoryFromDb = _dbContext.Categories.Find(category.Id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            categoryFromDb.Name = category.Name;

            await _dbContext.SaveChangesAsync();
            TempData["success"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        // Delete Action: Display confirmation page to delete a category
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Delete Action: Handle deletion of the category
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
