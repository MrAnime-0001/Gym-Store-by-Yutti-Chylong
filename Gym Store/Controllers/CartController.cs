using Gym_Store.Data;
using Gym_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Gym_Store.Helpers;

namespace Gym_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart); // Make sure Views/Cart/Index.cshtml exists
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
        public IActionResult Remove(int productId)
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                CartSessionHelper.SaveCart(HttpContext.Session, cart);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            return Json(new { count = cart.Sum(c => c.Quantity) });
        }
    }
}
