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

                List<WatchListItem> watchListItems = db
                    .WatchListItems
                    .Include(c => c.Item)
                    .Where(c => c.ApplicationUserID == userId)
                    .ToList();

                return View(watchListItems);
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<ActionResult> AddToWatchList(string id)
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

                addItem.WatchListItems = db
                    .WatchListItems
                    .Where(c => c.ItemID == addItem.ID)
                    .ToList();

                if (!addItem.BelongsTo(currentUser) && !addItem.InWatchList(currentUser) && addItem.ForSale)
                {
                    WatchListItem newWatchListItem = new WatchListItem(currentUser, addItem);
                    db.WatchListItems.Add(newWatchListItem);
                    db.SaveChanges();
                }

                return Redirect("/WatchList");
            }

        return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult RemoveFromWatchList(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                int itemId = Convert.ToInt32(id);
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
                WatchListItem removeWatchListItem = db
                    .WatchListItems
                    .Single(i => i.ItemID == itemId && i.ApplicationUser.Id == userId);
            
                db.WatchListItems.Remove(removeWatchListItem);
                db.SaveChanges();

                return Redirect("/WatchList");
            }
        
        return Redirect("/Account/Login");
        }
    }   
}