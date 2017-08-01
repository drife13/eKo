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
    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public OrdersController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            db = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                List<Order> orders = db
                    .Orders
                    .Where(c => c.ApplicationUserID == userId)
                    .OrderBy(c => c.OrderDate)
                    .ToList();

                return View(orders);
            }

            return Redirect("/Account/Login");
        }

        [HttpGet]
        //[Route("/Orders/ViewOrder/{id}")]
        public async Task<IActionResult> ViewOrder(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

                List<Order> orders = db
                    .Orders
                    .Include(o => o.Items)
                    .Where(o => o.ID == id && o.ApplicationUserID == userId)
                    .ToList();

                return View(orders);
            }
            return Redirect("/Account/Login");
        }
    }
}