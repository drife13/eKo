using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eko.Models;
using System.ComponentModel.DataAnnotations;

namespace Eko.Models.ItemViewModels
{
    public class SellItemViewModel
    {
        [Required]
        [Display(Name = "Listing Title")]
        public string Title { get; set; }

        [Required]
        public float Price { get; set; }

        [Required(ErrorMessage = "You must give your listing a description")]
        public string Description { get; set; }

        public SellItemViewModel() { }
    }
}
