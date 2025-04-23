using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class Customer
    {
        List<Order> orders = new List<Order>();
        public String Address { get;  }
        public String Name { get;  }

        public IOutOfStockClientStrategy ClientStrategy { get; private set;  }

        public SynchronizedCollection<Shop> ShopList;
        public event EventHandler<Order>? OnMakeOrder;
        public Customer(string Name, string Address, IOutOfStockClientStrategy outOfStockClientStrategy, SynchronizedCollection<Shop> shopList)
        {
            this.Name = Name;
            this.Address = Address;
            this.ClientStrategy = outOfStockClientStrategy;
            this.ShopList = shopList;
        }

        public void OutOfStockListener(Order order)
        {
            (var good, var shop) = ClientStrategy.HandleOutOfStock(order, ShopList);
            if (shop != null)
            {
                MakeOrder(good, shop);
            }
        }
        public void MakeOrder(Good good, Shop shop)
        {
            Order order = new Order(this, good, shop);
            orders.Add(order);
            shop.MakeOrderListener(order);
            OnMakeOrder.Invoke(this, order);     
        }
    }
}
