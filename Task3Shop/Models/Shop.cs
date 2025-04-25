using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Tools;

namespace Task3Shop.Models
{
    public class Shop
    {
        public event EventHandler<Order> OnOutOfStock;
        public string Name { get; } = string.Empty;
        public string Address { get; } = string.Empty;

        public ConcurrentDictionary<Good, int> Stock { get; set; }

        public IEnumerable<DeliveryService> deliveryServices { get; set; }

        public Shop(string name, string address, IEnumerable<DeliveryService> deliveries)
        {
            Name = name;
            Address = address;
            deliveryServices = deliveries;
        }

        public Shop(string name,  IEnumerable<DeliveryService> deliveries)
        {
            Name = name;
            deliveryServices = deliveries;
        }

        public bool GetOrderFromStock(Order order)
        {
            if (!Stock.ContainsKey(order.Good))
            {
                order.Customer.OutOfStockListener(order);
                OnOutOfStock?.Invoke(this, order);
                return false;
            }

            int goodQuantity = Stock[order.Good];

            if (goodQuantity <= 0)
            {
                order.Customer.OutOfStockListener(order);
                OnOutOfStock?.Invoke(this, order);
                return false;
            }
            else
            {
                if (Stock.TryUpdate(order.Good, goodQuantity - 1, goodQuantity))
                    return true;
                else
                    return GetOrderFromStock(order);
            }
        }

        public bool IsGoodAvailable(Good good)
        {
            if (Stock.ContainsKey(good))
                return Stock[good] > 0;
            else
                return false;
        }

        public void MakeOrder(Order order)
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
