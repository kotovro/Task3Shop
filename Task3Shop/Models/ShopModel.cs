using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class ShopModel
    {
        Dictionary<String, int> Stock { get; set; }

        ShopModel(Dictionary<String, int> stock)
        {
            Stock = stock;
        }
    }
}
