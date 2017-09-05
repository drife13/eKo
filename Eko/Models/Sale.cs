using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class Sale
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        public int ItemId { get; set; }
        public int ModelId { get; set; }
    }
}
