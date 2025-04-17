using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class StockItem
    {
        public GoodModel Good { get; }
        public int Quantity { get; set; }

        public StockItem(GoodModel good, int quality)
        {
            Good = good;
            Quantity = quality;
        }
    }
}
