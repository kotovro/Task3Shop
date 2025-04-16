using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    interface IOutOfStockClientStrategy
    {
        public void MakeOrder(String itemName, List<ShopModel> shopsList);
    }
}
