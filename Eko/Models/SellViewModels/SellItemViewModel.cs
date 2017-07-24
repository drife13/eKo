using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eko.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Eko.Models.ItemViewModels
{
    public class SellItemViewModel
    {
        [Required]
        [Display(Name = "Listing Title")]
        public string Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "You must give your listing a description")]
        public string Description { get; set; }

        [Required]
        public IList<IFormFile> Files { get; set; }

        public SellItemViewModel() { }
    }
}
