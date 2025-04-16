using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class OrderModel
    {
        CustomerModel customer { get; }
        GoodModel orderedGood { get; }

        OrderModel(CustomerModel customer, GoodModel order)
        {
            this.customer = customer;
            this.orderedGood = order;
        }
    }
}
