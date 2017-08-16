using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eko.Models.ItemViewModels
{
    public class EditItemViewModel : SellItemViewModel
    {
        public int ItemID { get; set; }

        public bool ForSale { get; set; }

        public EditItemViewModel() { }
        
        public EditItemViewModel(Item item, List<Category> categories) : base(categories)
        {
            ItemID = item.ID;
            ForSale = item.ForSale;
            Title = item.Title;
            Price = item.Price;
            Description = item.Description;
            Condition = item.Condition.ToString();
            CategoryID = item.CategoryID;
            Brand = item.Brand.Name;
            Model = item.Model.Name;
            Year = item.Year;
            // Files = item.files...
        }
    }
}
