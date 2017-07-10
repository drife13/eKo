using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class Item
    {
        public int ID { get; set; }
        public ApplicationUser Owner { get; set; }

        public string Title { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //public bool Sold { get; set; }
        //public bool Ended { get; set; }
        //public int Views { get; set; }
        //public int Watchers { get; set; }

        //public Condition Condition { get; set; }
        //public Make Make { get; set; }
        //public Model Model { get; set; }

        //public IList<Images> Images { get; set; }

        public Item()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
