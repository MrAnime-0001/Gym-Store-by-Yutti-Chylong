using Gym_Store.Data;
using Gym_Store.Helpers;
using Gym_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gym_Store.Controllers
{
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

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult Add([FromBody] ProductRequest request)
        {
            var product = _dbContext.Products.Find(request.ProductId);
            if (product == null)
            {
                return NotFound();
            }

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

        [HttpPost]
        [Route("api/cart/remove")]
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

        [HttpPost]
        [Route("api/cart/confirm")]
        public async Task<IActionResult> ConfirmPurchase()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "You must log in to confirm the purchase." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin") && !roles.Contains("Customer"))
            {
                return Forbid();
            }

            var cartItems = CartSessionHelper.GetCart(HttpContext.Session);
            if (cartItems == null || !cartItems.Any())
            {
                return Json(new { success = false, message = "Your cart is empty." });
            }

            // Calculate total amount
            decimal totalAmount = 0m;
            foreach (var item in cartItems)
            {
                totalAmount += item.Price * item.Quantity;
            }

            // Create Order entity
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = new List<OrderItem>()
            };

            // Add order items & update product stock quantities
            foreach (var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });

                // Update product quantity in database
                var product = await _dbContext.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    if (product.Quantity < 0)
                    {
                        product.Quantity = 0; // prevent negative stock
                    }
                    _dbContext.Products.Update(product);
                }
            }

            // Save order and product quantity changes to DB
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // Clear cart session
            CartSessionHelper.SaveCart(HttpContext.Session, new List<CartItem>());

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            return Json(new { count = cart.Sum(c => c.Quantity) });
        }
    }
}
