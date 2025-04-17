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

        public Dictionary<GoodModel, int> Stock { get; set; }

        public ShopModel(string name, string address)
        {
            Name = name;
            Address = address;
        }
        public ShopModel(Dictionary<GoodModel, int> stock, string name, string address)
        {
            Stock = stock;
            Name = name;
            Address = address;
        }

        public bool IsGoodAvailable(GoodModel good)
        {
            return Stock.TryGetValue(good, out int quantity) && quantity > 0;
        }
        public void SellGood(GoodModel good)
        {
            if (Stock.ContainsKey(good) && Stock[good] > 0)
            {
                Stock[good]--;
            }
        }
    }
}
