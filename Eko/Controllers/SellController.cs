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
using Microsoft.AspNetCore.Http;
using System.IO;

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

                Condition condition = (Condition) Enum.Parse(typeof(Condition), sellItemViewModel.Condition);

                Item newItem = new Item()
                {
                    Owner = currentUser,
                    OwnerId = currentUser.Id,
                    Title = sellItemViewModel.Title,
                    Price = sellItemViewModel.Price,
                    Description = sellItemViewModel.Description,
                    Condition = condition
                };
                db.Items.Add(newItem);

                db.SaveChanges();

                UploadImage(sellItemViewModel.Files, newItem);

                return Redirect("/Items/ViewItem/" + newItem.ID);
            }

            return View(sellItemViewModel);
        }

        public void UploadImage(IList<IFormFile> files, Item associatedItem)
        {
            foreach (IFormFile uploadedImage in files)
            {
                if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
                {
                    MemoryStream ms = new MemoryStream();
                    uploadedImage.OpenReadStream().CopyTo(ms);

                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                    Image imageEntity = new Image()
                    {
                        Id = Guid.NewGuid(),
                        Item = associatedItem,
                        Name = uploadedImage.Name,
                        Data = ms.ToArray(),
                        Width = image.Width,
                        Height = image.Height,
                        ContentType = uploadedImage.ContentType
                    };

                    db.Images.Add(imageEntity);
                }
            }

            db.SaveChanges();
        }
    }
}