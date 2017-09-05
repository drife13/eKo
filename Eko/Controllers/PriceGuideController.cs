using System;
using NuGet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eko.Models;
using Eko.Models.PriceGuideViewModels;
using Eko.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Eko.Controllers
{
    public class PriceGuideController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public PriceGuideController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            db = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Model> models = db
                .Models
                .Include(m => m.Brand)
                .Include(m => m.PriceHistory)
                .Where(m => m.PriceHistory.Count!=0)
                .OrderBy(m => m.Brand.Name)
                .ThenBy(m => m.Name)
                .ToList();

            return View(models);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Model(int id)
        {
            List<Model> existingModel = db
                .Models
                .Include(m => m.Brand)
                .Include(m => m.PriceHistory)
                .Where(m => m.PriceHistory.Count != 0)
                .Where(i => i.ID == id)
                .ToList();

            if (existingModel.Count != 0)
            {
                Model model = existingModel[0];

                List<Item> items = db
                    .Items
                    .Where(i => i.ForSale && i.ModelID == model.ID)
                    .OrderBy(i => i.CreatedDate)
                    .ToList();

                return View(new ModelViewModel(model, items));
            }

            return Redirect("/PriceGuide");
        }
    }
}