using Microsoft.AspNetCore.Http;
using Gym_Store.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gym_Store.Helpers
{
    public static class CartSessionHelper
    {
        private const string CartKey = "Cart";

        public static List<CartItem> GetCart(ISession session)
        {
            return session.GetObjectFromJson<List<CartItem>>(CartKey) ?? new List<CartItem>();
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            session.SetObjectAsJson(CartKey, cart);
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

            SaveCart(session, cart);
        }
    }
}
