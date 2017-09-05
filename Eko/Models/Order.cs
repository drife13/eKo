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

        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (Item item in Items)
                {
                    total += item.Price;
                }
                return total;
            }
            set { }
        }

        public Order() { }

        public Order(ApplicationUser user)
        {
            ApplicationUser = user;
            ApplicationUserID = user.Id;
            Items = new List<Item>();
            OrderDate = DateTime.Now;
        }
    }
}
