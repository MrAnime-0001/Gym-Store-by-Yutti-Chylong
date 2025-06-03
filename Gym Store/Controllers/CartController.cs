using Gym_Store.Data;
using Gym_Store.Helpers;
using Gym_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gym_Store.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cartItems = CartSessionHelper.GetCart(HttpContext.Session);

            var model = cartItems.Select(ci =>
            {
                var product = _dbContext.Products.Find(ci.ProductId);
                return new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = ci.Quantity
                };
            }).ToList();

            return View(model);
        }

        public class ProductRequest
        {
            public int ProductId { get; set; }
        }

        public class UpdateCartQuantityRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        [HttpPost("api/cart/add")]
        public IActionResult Add([FromBody] ProductRequest request)
        {
            var product = _dbContext.Products.Find(request.ProductId);
            if (product == null)
                return NotFound();

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            };

            CartSessionHelper.AddToCart(HttpContext.Session, cartItem);
            return Json(new { message = $"{product.Name} added to cart." });
        }

        [HttpPost("api/cart/remove")]
        public IActionResult Remove([FromBody] ProductRequest request)
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var item = cart.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (item != null)
            {
                cart.Remove(item);
                CartSessionHelper.SaveCart(HttpContext.Session, cart);
                return Ok(new { message = "Item removed from cart." });
            }

            return NotFound(new { message = "Item not found in cart." });
        }

        [HttpPost("api/cart/updateQuantity")]
        public IActionResult UpdateQuantity([FromBody] UpdateCartQuantityRequest request)
        {
            if (request.Quantity < 1)
                return BadRequest("Quantity must be at least 1.");

            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var item = cart.FirstOrDefault(x => x.ProductId == request.ProductId);

            if (item == null)
                return NotFound("Item not found in cart.");

            item.Quantity = request.Quantity;
            CartSessionHelper.SaveCart(HttpContext.Session, cart);
            return Ok();
        }

        [HttpPost("api/cart/confirm")]
        public async Task<IActionResult> ConfirmPurchase()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "You must log in to confirm the purchase." });

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin") && !roles.Contains("Customer"))
                return Forbid();

            var cartItems = CartSessionHelper.GetCart(HttpContext.Session);
            if (cartItems == null || !cartItems.Any())
                return Json(new { success = false, message = "Your cart is empty." });

            decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cartItems)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                    return Json(new { success = false, message = $"Product with ID {item.ProductId} no longer exists." });

                if (item.Quantity > product.Quantity)
                    return Json(new { success = false, message = $"Not enough stock for {product.Name}. Available: {product.Quantity}" });

                product.Quantity -= item.Quantity;
                _dbContext.Products.Update(product);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            CartSessionHelper.SaveCart(HttpContext.Session, new List<CartItem>());

            // ✅ Redirect to Receipt Page
            return Json(new { success = true, redirectUrl = Url.Action("Receipt", "Cart", new { id = order.Id }) });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            return Json(new { count = cart.Sum(c => c.Quantity) });
        }

        // ✅ Receipt Page (new)
        [HttpGet]
        public async Task<IActionResult> Receipt(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || (!isAdmin && order.UserId != user.Id))
                return NotFound();

            return View(order);
        }
    }
}
