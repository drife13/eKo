using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eko.Models;
using Eko.Models.CartViewModels;
using Eko.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eko.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public CartController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            db = dbContext;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                List<CartItem> cartItems = db
                    .CartItems
                    .Include(c => c.Item)
                    .Where(c => c.ApplicationUserID == userId)
                    .ToList();

                return View(new CartViewModel(cartItems));
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(string id)
        {
            int itemId = Convert.ToInt32(id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            Item addItem = db
                .Items
                .Include(i => i.Owner)
                .Single(i => i.ID == itemId);

            List<CartItem> existingCartItems = db
                .CartItems
                .Include(c => c.Item)
                .Where(c => c.Item.ID == itemId && c.ApplicationUserID == userId)
                .ToList();

            if (existingCartItems.Count == 0 && addItem.Owner.Id != userId)
            {
                CartItem newCartItem = new CartItem(currentUser, addItem);
                db.CartItems.Add(newCartItem);
                db.SaveChanges();
            }
            
            return Redirect("/Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(string id)
        {
            int itemId = Convert.ToInt32(id);
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartItem removeCartItem = db
                .CartItems
                .Single(i => i.ItemID == itemId && i.ApplicationUser.Id == userId);

            db.CartItems.Remove(removeCartItem);
            db.SaveChanges();

            return Redirect("/Cart");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int[] cartItemIds)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                List<CartItem> cartItems = db
                    .CartItems
                    .Include(c => c.Item)
                    .Where(c => c.ApplicationUserID == userId)
                    .ToList();

                var setCartItemIds = new HashSet<int>();
                foreach (CartItem item in cartItems)
                {
                    setCartItemIds.Add(item.Item.ID);
                }
                if (!setCartItemIds.SetEquals(cartItemIds))
                {
                    return Redirect("/Cart");
                }

                return View(new CartViewModel(cartItems));
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int[] cartItemIds)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                List<CartItem> cartItems = db
                    .CartItems
                    .Include(c => c.Item)
                    .Where(c => c.ApplicationUserID == userId)
                    .ToList();

                // Verify that posted cart IDs match those in user's cart.
                var setCartItemIds = new HashSet<int>();
                foreach (CartItem item in cartItems)
                {
                    setCartItemIds.Add(item.Item.ID);
                }
                if (!setCartItemIds.SetEquals(cartItemIds))
                {
                    return Redirect("/Cart");
                }

                Order newOrder = new Order(currentUser);
                
                foreach (int cartItemId in cartItemIds)
                {
                    foreach (var cartItem in db.CartItems.Where(c => c.Item.ID == cartItemId))
                    {
                        db.CartItems.Remove(cartItem);
                    }

                    foreach (var watchListItem in db.WatchListItems.Where(c => c.Item.ID == cartItemId))
                    {
                        db.WatchListItems.Remove(watchListItem);
                    }

                    Item item = db.Items.Single(i => i.ID == cartItemId);
                    item.ForSale = false;

                    newOrder.Items.Add(item);
                    newOrder.Total += item.Price;
                }
                db.Orders.Add(newOrder);

                db.SaveChanges();
                
                return Redirect("/Orders/ViewOrder/" + newOrder.ID);
            }

            return Redirect("/Account/Login");
        }
    }
}