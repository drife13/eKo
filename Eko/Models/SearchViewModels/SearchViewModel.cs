using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Eko.Models.SearchViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Search Term")]
        public string Query { get; set; }

        [Display(Name = "Category")]
        public int? CategoryID { get; set; }

        public string Brand { get; set; }

        public string Condition { get; set; }

        [Display(Name = "Minimum Year")]
        public int? YearMin { get; set; }
        [Display(Name = "Maximum Year")]
        public int? YearMax { get; set; }

        [Display(Name = "Minimum Price")]
        public decimal? PriceMin { get; set; }
        [Display(Name = "Maximum Price")]
        public decimal? PriceMax { get; set; }

        public string Sort { get; set; }

        public List<Item> Items { get; set; }

        public List<Category> SelectCategories { get; set; }
        public List<Brand> SelectBrands { get; set; }
        public List<Condition> SelectConditions { get; set; }
        
        public List<SelectListItem> Conditions { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Brands { get; set; }

        public List<SelectListItem> SortOptions { get; set; }

        public SearchViewModel()
        {
            SortOptions = new List<SelectListItem>();
            SortOptions.Add(new SelectListItem
            {
                Value = "lowtohigh",
                Text = "Price, low to high"
            });
            SortOptions.Add(new SelectListItem
            {
                Value = "hightolow",
                Text = "Price, high to low"
            });
            SortOptions.Add(new SelectListItem
            {
                Value = "recent",
                Text = "Recent"
            });
        }

        public void Search(List<Item> items)
        {
            Items = items;
            
            if (CategoryID.HasValue)
            {
                Items = Items.Where(i =>
                i.Category.GrandParentId == CategoryID ||
                i.Category.ParentId == CategoryID ||
                i.Category.ID == CategoryID)
                .ToList();
            }

            if (!String.IsNullOrEmpty(Sort))
            {
                if (Sort == "hightolow")
                {
                    Items = Items.OrderByDescending(i => i.Price).ToList();
                }
                if (Sort == "lowtohigh")
                {
                    Items = Items.OrderBy(i => i.Price).ToList();
                }
                if (Sort == "recent")
                {
                    Items = Items.OrderByDescending(i => i.CreatedDate).ToList();
                }
            }

            if (PriceMin.HasValue)
            {
                Items = Items.Where(i => i.Price >= PriceMin).ToList();
            }

            if (PriceMax.HasValue)
            {
                Items = Items.Where(i => i.Price <= PriceMax).ToList();
            }

            if (YearMin.HasValue)
            {
                Items = Items.Where(i => i.Year >= YearMin).ToList();
            }

            if (YearMax.HasValue)
            {
                Items = Items.Where(i => i.Year <= YearMax).ToList();
            }

            if (!String.IsNullOrEmpty(Condition))
            {
                Condition condition = (Condition)Enum.Parse(typeof(Condition), Condition);
                Items = Items.Where(i => i.Condition == condition).ToList();
            }

            if (!String.IsNullOrEmpty(Brand))
            {
                Items = Items.Where(i => i.Brand.URLName == Brand).ToList();
            }

            if (!String.IsNullOrWhiteSpace(Query))
            {
                string query = new Regex("[^a-zA-Z0-9 -]").Replace(Query, "");
                RegexOptions options = RegexOptions.IgnoreCase;

                foreach (string term in query.Split(' '))
                {
                    string word = "\\b" + term + "\\b";
                    Items = Items.Where(i =>
                        Regex.IsMatch(i.Title, word, options) ||
                        Regex.IsMatch(i.Category.FullName, word, options) ||
                        Regex.IsMatch(i.Model.Name, word, options) ||
                        Regex.IsMatch(i.Brand.Name, word, options) ||
                        Regex.IsMatch(i.Description, word, options)
                        ).ToList();
                }
            }

            SelectCategories = new List<Category>();
            SelectBrands = new List<Brand>();
            SelectConditions = new List<Condition>();
            foreach (Item item in items)
            {
                if (!SelectCategories.Contains(item.Category))
                {
                    SelectCategories.Add(item.Category);
                }

                if (!SelectBrands.Contains(item.Brand))
                {
                    SelectBrands.Add(item.Brand);
                }

                if (!SelectConditions.Contains(item.Condition))
                {
                    SelectConditions.Add(item.Condition);
                }
            }

            Conditions = new List<SelectListItem>();
            Conditions.Add(new SelectListItem { Value = "", Text = "" });
            foreach (Condition condition in SelectConditions)
            {
                Conditions.Add(new SelectListItem
                {
                    Value = condition.ToString(),
                    Text = condition.ToString()
                });
            }

            Brands = new List<SelectListItem>();
            Brands.Add(new SelectListItem { Value = "", Text = "" });
            if (SelectBrands.Count != 0)
            {
                foreach (Brand brand in SelectBrands.OrderBy(i => i.Name))
                {
                    Brands.Add(new SelectListItem
                    {
                        Value = brand.URLName,
                        Text = brand.Name
                    });
                }
            }

            Categories = new List<SelectListItem>();
            Categories.Add(new SelectListItem { Value = "", Text = "" });
            foreach (Category category in SelectCategories.OrderBy(i => i.FullName))
            {
                Categories.Add(new SelectListItem
                {
                    Value = category.ID.ToString(),
                    Text = category.FullName
                });
            }
        }
    }
}
