using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class ProductHistory
    {
        public int ID { get; set; }

        public Model Model { get; set; }
        public int ModelID { get; set; }

        public Brand Brand { get; set; }
        public int BrandID { get; set; }

        public Category Category { get; set; }
        public int CategoryID { get; set; }

        public List<Order> Orders { get; set; }
        //public List<decimal> SalePrices { get; set; }
        //public List<DateTime> SaleDates { get; set; }
    }
}
