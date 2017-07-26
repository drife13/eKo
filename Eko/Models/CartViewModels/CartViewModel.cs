using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models.CartViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Cart { get; set; }

        public decimal Subtotal { get; set; }

        public CartViewModel(List<CartItem> cart)
        {
            Cart = cart;
            foreach (CartItem item in cart)
            {
                Subtotal += item.Item.Price;
            }
        }
    }
}
