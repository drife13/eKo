using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class Order
    {
        public int ID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserID { get; set; }

        public IList<Item> Items { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; }

        public Order() { }

        public Order(ApplicationUser user)
        {
            ApplicationUser = user;
            ApplicationUserID = user.Id;
            Items = new List<Item>();
            Total = 0;
            OrderDate = DateTime.Now;
        }
    }
}
