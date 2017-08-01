using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class WatchListItem
    {
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ItemID { get; set; }
        public Item Item { get; set; }

        public WatchListItem() { }

        public WatchListItem(ApplicationUser user, Item item)
        {
            ApplicationUserID = user.Id;
            ApplicationUser = user;
            ItemID = item.ID;
            Item = item;
        }
    }
}
