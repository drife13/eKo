using System;
using System.Collections.Generic;
using System.Linq;
using Eko.Models.ItemViewModels;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class Item
    {
        public int ID { get; set; }

        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public Condition Condition { get; set; }

        public Brand Brand { get; set; }
        public int BrandID { get; set; }

        public Model Model { get; set; }
        public int ModelID { get; set; }

        public int Year { get; set; }

        public Category Category { get; set; }
        public int CategoryID { get; set; }
        
        public bool ForSale { get; set; }
            //get { if (Sold) { return false } else { return forSale } }
            //set { } }
        public bool Sold { get; set; }
        //public int Views { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime SoldDate { get; set; }

        //public IEnumerable<Guid> ImageIds { get; set; }
        public IList<CartItem> CartItems { get; set; }
        public IList<WatchListItem> WatchListItems { get; set; }

        public Item()
        {
            ForSale = true;
            Sold = false;
            CreatedDate = DateTime.Now;
        }

        public bool BelongsTo(ApplicationUser user)
        {
            return Owner.Id == user.Id;
        }

        public bool InCart(ApplicationUser user)
        {
            List<CartItem> cartItem = CartItems
                .Where(i => i.ApplicationUserID == user.Id)
                .ToList();

            return cartItem.Count != 0;
        }

        public bool InWatchList(ApplicationUser user)
        {
            List<WatchListItem> watchListItem = WatchListItems
                .Where(i => i.ApplicationUserID == user.Id)
                .ToList();

            return watchListItem.Count != 0;
        }

        public void EditProperties(EditItemViewModel e, Category category, Brand brand, Model model)
        {
            Title = e.Title;
            Price = e.Price;
            Description = e.Description;
            Condition = (Condition)Enum.Parse(typeof(Condition), e.Condition);
            Category = category;
            Brand = brand;
            Model = model;
            Year = e.Year;
        }

        public Sale Sell(Order order)
        {
            ForSale = false;
            Sold = true;
            SoldDate = order.OrderDate;

            Sale sale = new Sale()
            {
                Date = SoldDate,
                Price = Price,
                ItemId = ID,
                ModelId = ModelID
            };

            Model.PriceHistory.Add(sale);
            order.Items.Add(this);

            return sale;
        }
    }
}
