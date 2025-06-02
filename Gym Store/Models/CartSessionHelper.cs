using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Gym_Store.Models
{
    public static class CartSessionHelper
    {
        private const string CartKey = "Cart";

        public static List<CartItem> GetCart(ISession session)
        {
            var cartJson = session.GetString(CartKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            session.SetString(CartKey, cartJson);
        }

        public static void AddToCart(ISession session, CartItem item)
        {
            var cart = GetCart(session);
            var existing = cart.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            Console.WriteLine(JsonSerializer.Serialize(cart)); // Debugging
            SaveCart(session, cart);
        }
    }
}
