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
    public class ItemsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public ItemsController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
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
                
                List<Item> items = db
                    .Items
                    .Where(i => i.Owner.Id == userId)
                    .ToList();

                // OPTIONAL: Make sure they see something
                if (items.Count == 0) // They have no related products so just send all of them
                    items = db.Items.ToList();

                // only send the products related to that user
                return View(items);
            }
            // User is not authenticated, send them all products
            return View(db.Items.ToList());
        }

        [HttpGet]
        [Route("/Items/{id}")]
        public IActionResult ViewItem(int id)
        {
            return View(db.Items.Include(i => i.Owner).Single(i => i.ID == id));
        }

        [HttpPost]
        public async Task<ActionResult> AddToWatchList(string id)
        {
            int itemId = Convert.ToInt32(id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            WatchListItem newWatchListItem = new WatchListItem()
            {
                ApplicationUser = currentUser,
                Item = db.Items.Single(i => i.ID == itemId)
            };
            db.WatchListItems.Add(newWatchListItem);

            db.SaveChanges();

            return Redirect("/WatchList");
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(string id)
        {
            int itemId = Convert.ToInt32(id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            CartItem newCartItem = new CartItem()
            {
                ApplicationUser = currentUser,
                Item = db.Items.Single(i => i.ID == itemId)
            };
            db.CartItems.Add(newCartItem);

            db.SaveChanges();

            return Redirect("/Cart");
        }
    }
}