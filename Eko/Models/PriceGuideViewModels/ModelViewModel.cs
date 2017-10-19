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

        public DateTime[] Dates { get; set; }

        public decimal[] Prices { get; set; }

        public decimal Quart1 { get; set; }
        public decimal Quart2 { get; set; }
        public decimal Quart3 { get; set; }

        public string EstimatedValue { get; set; }
        
        public ModelViewModel() { }

        public ModelViewModel(Model model, List<Item> items)
        {
            Model = model;
            Brand = model.Brand;
            Items = items;

            Dates = new DateTime[Model.PriceHistory.Count];
            Prices = new decimal[Model.PriceHistory.Count];

            int i = 0;
            foreach (Sale sale in Model.PriceHistory.OrderBy(s => s.Date))
            {
                Dates[i] = sale.Date;
                Prices[i] = sale.Price;
                i++;
            }

            List<decimal> list = Prices.OrderBy(x => x).ToList();

            Quart2 = Median(list);
            Quart1 = Median(list.Where(x => x < Quart2).ToList());
            Quart3 = Median(list.Where(x => x > Quart2).ToList());

            if (Quart1 != Quart3)
            {
                EstimatedValue = "$" + Quart1.ToString() + " - $" + Quart3.ToString();
            }
            else
            {
                EstimatedValue = "$" + Quart2.ToString();
            }
        }

        public decimal Median(List<decimal> list)
        {
            decimal median = 0m;

            if (list.Count % 2 == 0 && list.Count > 0)
            {
                median = (list[list.Count / 2 - 1] + list[list.Count / 2]) / 2;
            }
            else if (list.Count > 0)
            {
                median = list[(list.Count - 1) / 2];
            }

            return median;
        }
    }
}
