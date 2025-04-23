using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class Shop
    {
        public event EventHandler<Order> OnOutOfStock;
        public string Name { get; } = string.Empty;
        public string Address { get; } = string.Empty;

        public Dictionary<Good, int> Stock { get; set; }

        public SynchronizedCollection<DeliveryService> deliveryServices { get; set; }

        public Shop(string name, string address, SynchronizedCollection<DeliveryService> deliveries)
        {
            Name = name;
            Address = address;
            deliveryServices = deliveries;
        }

        public bool GetOrderFromStock(Order order)
        {
            lock (Stock)
            {
                if (IsGoodAvailable(order.Good))
                {
                    Stock[order.Good]--;
                    return true;
                }
            }
            order.Customer.OutOfStockListener(order);
            OnOutOfStock.Invoke(this, order);
            return false;
        }

        public bool IsGoodAvailable(Good good)
        {
            return Stock[good] > 0;
        }

        public void MakeOrderListener(Order order)
        {
            List<DeliveryService> shuffledServices = Toolkit.GetShuffledList(deliveryServices);
            foreach (DeliveryService svc in shuffledServices)
            {
                if (svc.RequestFastDelivery(order))
                {
                    return;
                }
            }

            shuffledServices.First().RequestLateDelivery(order);
            
        }
        //public void SellGood(GoodModel good)
        //{
        //    Stock[good]--;
        //}
    }
}
