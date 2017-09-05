using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models.PriceGuideViewModels
{
    public class ModelViewModel
    {
        public Model Model { get; set; }

        public Brand Brand { get; set; }

        public IList<Item> Items { get; set; }

        public ModelViewModel() { }

        public ModelViewModel(Model model, List<Item> items)
        {
            Model = model;
            Brand = model.Brand;
            Items = items;
        }
    }
}
