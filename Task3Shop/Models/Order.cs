using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class Order
    {
        public Customer Customer { get; }
        public Good Good { get; }

        public Shop Shop { get;  }
        public Order(Customer customer, Good order, Shop shop)
        {
            this.Customer = customer;
            this.Good = order;
            this.Shop = shop;
        }
    }
}
