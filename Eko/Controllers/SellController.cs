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
    public class SellController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public SellController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            db = dbContext;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                SellItemViewModel sellItemViewModel = new SellItemViewModel();
                return View(sellItemViewModel);
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(SellItemViewModel sellItemViewModel)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                Item newItem = new Item()
                {
                    Owner = currentUser,
                    Title = sellItemViewModel.Title,
                    Price = sellItemViewModel.Price,
                    Description = sellItemViewModel.Description,
                };
                db.Items.Add(newItem);

                db.SaveChanges();

                return Redirect("/Items/" + newItem.ID);
            }

            return View(sellItemViewModel);
        }
    }
}