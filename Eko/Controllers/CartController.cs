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
            if (User.Identity.IsAuthenticated)
            {
                int itemId = Convert.ToInt32(id);

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                Item addItem = db
                    .Items
                    .Include(i => i.Owner)
                    .Single(i => i.ID == itemId);

                addItem.CartItems = db
                    .CartItems
                    .Where(c => c.ItemID == addItem.ID)
                    .ToList();

                if (!addItem.BelongsTo(currentUser) && !addItem.InCart(currentUser) && addItem.ForSale)
                {
                    CartItem newCartItem = new CartItem(currentUser, addItem);
                    db.CartItems.Add(newCartItem);
                    db.SaveChanges();
                }

                return Redirect("/Cart");
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(string id)
        {
            if (User.Identity.IsAuthenticated)
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

            return Redirect("/Account/Login");
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