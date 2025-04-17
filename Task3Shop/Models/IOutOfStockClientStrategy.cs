using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public interface IOutOfStockClientStrategy
    {
        public void HandleOutOfStock(GoodModel good, List<ShopModel> shopsList);
    }
}
