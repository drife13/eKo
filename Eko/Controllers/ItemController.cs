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
    public class ItemController : Controller
    {
        private ApplicationDbContext db;

        public ItemController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        //[AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //var userId = UserManager<IdentityUser>.GetUserId(User);
                //ApplicationUser currentUser = await UserManager.FindByIdAsync(userId);
                
                List<UserItem> items = db
                    .UserItems
                    .Include(item => item.Item)
                    .Where(ui => ui.ApplicationUser.Id == userId)
                    .ToList();

                // OPTIONAL: Make sure they see something
                //if (items.Count == 0) // They have no related products so just send all of them
                //    items = db.UserItems.Include(item => item.Item).ToList();

                // only send the products related to that user
                return View(items);
            }
            // User is not authenticated, send them all products
            return View(db.UserItems.Include(item => item.Item).ToList());
        }

        public IActionResult Sell()
        {
            if (User.Identity.IsAuthenticated)
            {
                SellItemViewModel sellItemViewModel = new SellItemViewModel();
                return View(sellItemViewModel);
            }

            return Redirect("/Account/Login/");
        }

        [HttpPost]
        public IActionResult Sell(SellItemViewModel sellItemViewModel)
        {
            if (ModelState.IsValid)
            {
                //ClaimsPrincipal user = User;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

                Item newItem = new Item()
                {
                    Owner = currentUser,
                    Title = sellItemViewModel.Title,
                    Price = sellItemViewModel.Price,
                    Description = sellItemViewModel.Description,
                };
                db.Items.Add(newItem);

                UserItem newUserItem = new UserItem
                {
                    ApplicationUser = currentUser,
                    Item = newItem
                };
                db.UserItems.Add(newUserItem);

                db.SaveChanges();

                return Redirect("/Item/");
            }

            return View(sellItemViewModel);
        }
    }
}