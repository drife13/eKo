using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models.CartViewModels
{
    public class CartViewModel
    {
        public IList<CartItem> Cart { get; set; }

        public decimal Subtotal { get; set; }

        public CartViewModel(IList<CartItem> cart)
        {
            Cart = cart;
            foreach (CartItem item in cart)
            {
                Subtotal += item.Item.Price;
            }
        }
    }}
