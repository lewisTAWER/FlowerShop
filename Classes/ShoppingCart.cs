using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using FlowerShop.Classes;
using System.Windows.Controls;
using System.Windows;
using System.Linq;

namespace FlowerShop.Classes
{
    public class ShoppingCart
    {
        private static readonly Lazy<ShoppingCart> lazy =
            new Lazy<ShoppingCart>(() => new ShoppingCart());

        public static ShoppingCart Instance { get { return lazy.Value; } }

        public List<CartItem> CartItems { get; set; }

        public event EventHandler CartUpdated;

        public ShoppingCart()
        {
            CartItems = new List<CartItem>();
        }

        public void AddItem(item_products item, int quantity)
        {
            CartItem existingItem = CartItems.FirstOrDefault(ci => ci.Item == item);

            if (existingItem != null)
            {
                if (quantity <= 0) CartItems.Remove(existingItem);
                else existingItem.Quantity = quantity;
            }
            else if (quantity > 0)
            {
                CartItem cartItem = new CartItem(item, quantity);
                CartItems.Add(cartItem);
            }

            OnCartUpdated();
        }

        public void Clear()
        {
            CartItems.Clear();
            OnCartUpdated();
        }

        public decimal GetTotalCost()
        {
            return CartItems.Sum(item => item.GetTotalCost());
        }

        protected virtual void OnCartUpdated()
        {
            CartUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CartItem
    {
        public item_products Item { get; set; }
        public int Quantity { get; set; }

        public CartItem(item_products item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public decimal GetTotalCost()
        {
            return Item.ProductsPrice * Quantity;
        }
    }
}
