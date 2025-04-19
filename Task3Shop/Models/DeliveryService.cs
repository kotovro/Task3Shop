using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class DeliveryService
    {
        public int TotalCars { get; }
        public int AvailableCars { get; private set; }
        public string ServiceName { get; set; }

        private PriorityQueue<Order, int> DelayedDeliveries = new();
        private SynchronizedCollection<Order> OrdersOnDelivery = new ();
        private Lock _locker = new ();

        public void FinishDelivery(object shop, Order order)
        {
            OrdersOnDelivery.Remove(order);

            lock (_locker)
            {
                AvailableCars++;
            }
            if (DelayedDeliveries.Count > 0)
            {                
                lock (DelayedDeliveries)
                {
                    order = (Order)DelayedDeliveries.Dequeue();
                    if (!RequestFastDelivery(order))
                    {
                        DelayedDeliveries.Enqueue(order, 0);
                    }
                }
            }
        }

        public void RequestLateDelivery(Order order)
        {
            DelayedDeliveries.Enqueue(order, 10);
        }

        public bool RequestFastDelivery(Order order)
        {

            if (AvailableCars <= 0)
            {
                return false;
            }

            lock (_locker)
            {
                AvailableCars--;
            }

            lock (OrdersOnDelivery.SyncRoot)
            {
                OrdersOnDelivery.Append(order);
            }
            
            order.Shop.OnOutOfStock += FinishDelivery;
            return true;
        }

        public DeliveryService(int totalCars, string ServiceName)
        {
            this.TotalCars = totalCars;
            this.AvailableCars = totalCars;
            this.ServiceName = ServiceName;
        }

    }
}
