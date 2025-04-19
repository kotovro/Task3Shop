﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class DontBuyAnythingStrategy : IOutOfStockClientStrategy
    {
        public (Good, Shop?) HandleOutOfStock(Order order, SynchronizedCollection<Shop> shopsList)
        {
            return (order.Good, null);
        }
    }
}
