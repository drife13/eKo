using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eko.Models;
using Eko.Models.ItemViewModels;
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

                List<CartItem> cart = db
                    .CartItems
                    .Include(c => c.Item)
                    .Where(c => c.ApplicationUserID == userId)
                    .ToList();

                return View(cart);
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(string id)
        {
            int itemId = Convert.ToInt32(id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            IList<CartItem> existingItems = db
                .CartItems
                .Where(ci => ci.ApplicationUserID == userId)
                .Where(ci => ci.ItemID == itemId)
                .ToList();

            if (existingItems.Count == 0)
            {
                CartItem newCartItem = new CartItem()
                {
                    ApplicationUser = currentUser,
                    Item = db.Items.Single(i => i.ID == itemId)
                };
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
                .Single(i => i.ItemID == itemId && i.ApplicationUserID == userId);

            db.CartItems.Remove(removeCartItem);
            db.SaveChanges();

            return Redirect("/Cart");
        }
    }
}