using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Tools;

namespace Task3Shop.Models
{
    class MakeNewOrderHereStrategy : IOutOfStockClientStrategy
    {
        public (Good, Shop?) HandleOutOfStock(Order order, IEnumerable<Shop> shopsList)
        {
            var shop = order.Shop;

            List<Good> shuffledList = Toolkit.GetShuffledList(order.Shop.Stock.Keys);
            foreach (Good good in shuffledList)
            {
                if (shop.IsGoodAvailable(order.Good))
                {
                    return (order.Good, shop);
                }
            }
            return (order.Good, null);
        }
    }
}
