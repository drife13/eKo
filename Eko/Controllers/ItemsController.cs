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
            string userId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier).Value : "";

            Item item = db
                .Items
                .Include(i => i.Owner)
                .Include(i => i.Brand)
                .Include(i => i.Model)
                .Include(i => i.Category)
                .Single(i => i.ID == id);

            List<Guid> imageIds = db.Images.Where(m => m.Item.ID == id).Select(m => m.Id).ToList();
            int watchers = db.WatchListItems.Where(i => i.ItemID == id).ToList().Count;

            bool owner = item.OwnerId == userId ? true : false;
            
            ViewItemViewModel viewItemViewModel = new ViewItemViewModel()
            {
                Item = item,
                Owner = owner,
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

        [HttpGet]
        //[Route("/Items/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                List<Item> existingItems = db
                    .Items
                    .Include(i => i.Owner)
                    .Include(i => i.Brand)
                    .Include(i => i.Model)
                    .Include(i => i.Category)
                    .Where(i => i.Sold == false && i.ID == id)
                    .ToList();

                if (existingItems.Count != 0)
                {
                    Item item = existingItems[0];
                    if (item.OwnerId == userId)
                    {
                        EditItemViewModel editItemViewModel = new EditItemViewModel(item, db.Categories.ToList());

                        return View(editItemViewModel);
                    }
                }

                return ViewItem(id);
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult Edit(EditItemViewModel editItemViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                Category category = db.Categories.Single(c => c.ID == editItemViewModel.CategoryID);

                List<Brand> existingBrands = db.Brands.Where(b => b.Name == editItemViewModel.Brand).ToList();
                Brand brand = new Brand();
                if (existingBrands.Count == 0)
                {
                    brand = new Brand() { Name = editItemViewModel.Brand };
                    db.Brands.Add(brand);
                    db.SaveChanges();
                }
                else
                {
                    brand = existingBrands[0];
                }

                List<Model> existingModels = db
                    .Models
                    .Where(m => m.Name == editItemViewModel.Model && m.Brand == brand)
                    .ToList();
                Model model = new Model();
                if (existingModels.Count == 0)
                {
                    model = new Model() { Name = editItemViewModel.Model, Brand = brand };
                    db.Models.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    model = existingModels[0];
                }

                Item item = db.Items.Single(i => i.ID == editItemViewModel.ItemID);
                item.EditProperties(editItemViewModel, category, brand, model);
                db.SaveChanges();

                return Redirect("/Items/ViewItem/" + item.ID);
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult RemoveFromStore(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Item item = db.Items.Single(i => i.ID == id);
                item.ForSale = false;
                db.SaveChanges();
                
                return Redirect("/Items/MyStore");
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult AddToStore(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Item item = db.Items.Single(i => i.ID == id);
                item.ForSale = true;
                db.SaveChanges();

                return Redirect("/Items/MyStore");
            }

            return Redirect("/Account/Login");
        }
    }
}