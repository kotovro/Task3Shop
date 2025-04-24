using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Tools;

namespace Task3Shop.Models
{
    class MakeNewOrderAnywhereStrategy : IOutOfStockClientStrategy
    {
        public (Good, Shop?) HandleOutOfStock(Order order, IEnumerable<Shop> shopsList)
        {
            var shuffledList = Toolkit.GetShuffledList(shopsList);
            foreach (Shop shop in shuffledList)
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
