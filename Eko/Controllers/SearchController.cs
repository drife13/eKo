using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eko.Models;
using Eko.Models.SearchViewModels;
using Eko.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eko.Controllers
{
    public class SearchController : Controller
    {
        private List<Item> AllSearchItems = new List<Item>();

        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public SearchController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            db = dbContext;
            if (AllSearchItems.Count==0)
            {
                AllSearchItems = db
                    .Items
                    .Include(i => i.Category)
                    .Include(i => i.Brand)
                    .Include(i => i.Model)
                    .Where(i => i.ForSale)
                    .ToList();
            }
        }

        [AllowAnonymous]
        public IActionResult Index(SearchViewModel searchViewModel)
        {
            searchViewModel.Search(AllSearchItems);

            return View(searchViewModel);
        }
    }
}