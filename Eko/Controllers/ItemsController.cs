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
using System.IO;

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
        public IActionResult Index()
        {
            List<Item> items = db
                .Items
                .Include(i => i.Owner)
                .Where(i => i.ForSale == true)
                .ToList();

            return View(items);
        }

        public IActionResult MyStore()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                List<Item> items = db
                    .Items
                    .Where(i => i.Owner.Id == userId)
                    .ToList();

                // only send the products related to that user
                return View(items);
            }
            
            return Redirect("/Account/Login");
        }

        [HttpGet]
        //[Route("/Orders/ViewItem/{id}")]
        public IActionResult ViewItem(int id)
        {
            Item item = db
                .Items
                .Include(i => i.Owner)
                .Include(i => i.Brand)
                .Include(i => i.Model)
                .Include(i => i.Category)
                .Single(i => i.ID == id);
            List<Guid> imageIds = db.Images.Where(m => m.Item.ID == id).Select(m => m.Id).ToList();
            int watchers = db.WatchListItems.Where(i => i.ItemID == id).ToList().Count;

            ViewItemViewModel viewItemViewModel = new ViewItemViewModel()
            {
                Item = item,
                ImageIds = imageIds,
                Watchers = watchers
            };

            return View(viewItemViewModel);
        }

        [HttpGet]
        public FileStreamResult ViewImage(Guid id)
        {
            Image image = db.Images.FirstOrDefault(m => m.Id == id);

            MemoryStream ms = new MemoryStream(image.Data);

            return new FileStreamResult(ms, image.ContentType);
        }
    }
}