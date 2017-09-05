using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Eko.Models.ItemViewModels
{
    public class SellItemViewModel
    {
        [Required]
        [Display(Name = "Listing Title")]
        public string Title { get; set; }

        [Required]
        [Range(0, 1e7, ErrorMessage = "Please enter a price greater than ${1}")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "You must give your listing a description.")]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must specify your item's condition.")]
        public string Condition { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "You must specify your item's brand.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "You must specify your item's model.")]
        public string Model { get; set; }

        [Required]
        [Range(1700, 2018, ErrorMessage = "You must specify your item's production year.")]
        public int Year { get; set; }

        [Required]
        public IList<IFormFile> Files { get; set; }

        public List<SelectListItem> Conditions { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public SellItemViewModel() { }

        public SellItemViewModel(List<Category> categories)
        {
            Conditions = new List<SelectListItem>();
            foreach (Condition condition in Enum.GetValues(typeof(Condition)))
            {
                Conditions.Add(new SelectListItem
                {
                    Value = condition.ToString(),
                    Text = condition.ToString()
                });
            }

            Categories = new List<SelectListItem>();
            foreach (Category category in categories.OrderBy(c => c.FullName))
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
