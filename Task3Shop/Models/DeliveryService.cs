using HarfBuzzSharp;
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
        private const int DISTANCE = 20;
        public int TotalCars { get; }
        public string ServiceName { get; set; }

        public List<DeliveryServiceVehicle> Vehicles = new ();
        private PriorityQueue<Order, int> DelayedDeliveries = new();
        private Lock _locker = new ();
        public DeliveryService(int totalCars, string ServiceName, int speed)
        {
            this.TotalCars = totalCars;
            this.ServiceName = ServiceName;
            for (int i = 0; i < TotalCars; i++)
            {
                Vehicles.Add(new DeliveryServiceVehicle(speed));
            }
        }

        public void FinishDelivery(DeliveryServiceVehicle vehicle)
        {
            lock (_locker)
            {
                vehicle.Order = null;
                vehicle.CurrentDirection = DeliveryServiceVehicle.Direction.ToBase;
                vehicle.Distance = GetDistanceToBase(vehicle);
            }
            if (DelayedDeliveries.Count > 0)
            {                
                lock (DelayedDeliveries)
                {
                    Order order = (Order)DelayedDeliveries.Dequeue();
                    if (!RequestFastDelivery(order))
                    {
                        DelayedDeliveries.Enqueue(order, 0);
                    }
                }
            }
        }

        private int GetDistanceToBase(DeliveryServiceVehicle vehicle) => DISTANCE;
        private int GetDistanceToClient(DeliveryServiceVehicle vehicle) => DISTANCE;
        private int GetDistanceToShop(DeliveryServiceVehicle vehicle) => DISTANCE;

        public void RequestLateDelivery(Order order)
        {
            DelayedDeliveries.Enqueue(order, 10);
        }

        public bool RequestFastDelivery(Order order)
        {

            if (!Vehicles.Any(v => v.Order == null))
            {
                return false;
            }

            lock (_locker)
            {
                DeliveryServiceVehicle vehicle = Vehicles.First(v => v.Order == null);
                vehicle.Order = order;
                vehicle.CurrentDirection = DeliveryServiceVehicle.Direction.ToShop;
                vehicle.Distance = GetDistanceToShop(vehicle);
            }
            return true;
        }

        public void TakeGoodFromShop(DeliveryServiceVehicle vehicle)
        {
            if (vehicle.Order.Shop.GetOrderFromStock(vehicle.Order))
            {
                vehicle.CurrentDirection = DeliveryServiceVehicle.Direction.ToClient;
                vehicle.Distance = GetDistanceToClient(vehicle);
            }
            else
               FinishDelivery(vehicle);
        }
    }
}
