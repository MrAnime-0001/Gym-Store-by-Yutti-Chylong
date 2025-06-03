using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Store.Controllers
{
    [Authorize]
    public class ReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReceiptsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge(); // force login

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            List<Order> orders;
            if (isAdmin)
            {
                orders = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync();

                var userIds = orders.Select(o => o.UserId).Distinct();
                var userEmails = await _context.Users
                    .Where(u => userIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id, u => u.Email);

                if (userEmails.Any())
                    ViewData["UserEmails"] = userEmails;
            }
            else
            {
                orders = await _context.Orders
                    .Where(o => o.UserId == user.Id)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync();
            }

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge(); // force login

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || (!isAdmin && order.UserId != user.Id))
                return NotFound();

            return View(order);
        }
    }
}
