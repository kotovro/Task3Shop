using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class ShopModel
    {
        public String Name { get; } = string.Empty;
        public String Address { get; } = string.Empty;

        public List<StockItem> Stock { get; set; }

        public ShopModel(string name, string address)
        {
            Name = name;
            Address = address;
        }
        

        public bool IsGoodAvailable(GoodModel good)
        {
            return Stock.Any(item => item.Good.Equals(good) && item.Quantity > 0);
        }
        public void SellGood(GoodModel good)
        {
            var item = Stock.FirstOrDefault(i => i.Good.Equals(good));

            if (item != null && item.Quantity > 0)
            {
                item.Quantity--;
            }
        }
    }
}
