using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eko.Models;
using Eko.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eko.Controllers
{
    public class WatchListController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public WatchListController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
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

                List<WatchListItem> watchList = db
                    .WatchListItems
                    .Include(c => c.Item)
                    .Where(c => c.ApplicationUserID == userId)
                    .ToList();

                return View(watchList);
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<ActionResult> AddToWatchList(string id)
        {
            int itemId = Convert.ToInt32(id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            IList<WatchListItem> existingWatchListItems = db
                .WatchListItems
                .Where(ci => ci.ApplicationUserID == userId)
                .Where(ci => ci.ItemID == itemId)
                .ToList();

            IList<Item> userItems = db
                .Items
                .Where(ci => ci.Owner.Id == userId)
                .Where(ci => ci.ID == itemId)
                .ToList();

            if (existingWatchListItems.Count == 0 && userItems.Count == 0)
            {
                WatchListItem newWatchListItem = new WatchListItem()
                {
                    ApplicationUser = currentUser,
                    Item = db.Items.Single(i => i.ID == itemId)
                };
                db.WatchListItems.Add(newWatchListItem);
                db.SaveChanges();
            }

            return Redirect("/WatchList");
        }

        [HttpPost]
        public IActionResult RemoveFromWatchList(string id)
        {
            int itemId = Convert.ToInt32(id);
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            WatchListItem removeWatchListItem = db
                .WatchListItems
                .Single(i => i.ItemID == itemId && i.ApplicationUserID == userId);
            
            db.WatchListItems.Remove(removeWatchListItem);
            db.SaveChanges();

            return Redirect("/WatchList");
        }
    }
}