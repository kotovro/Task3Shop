using HarfBuzzSharp;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
        public event EventHandler<Order> OnOrderTaken;
        public event EventHandler<Order> OnOrderDeliveryFinished;
        public event EventHandler<Order> OnOrderDeliveryStarted;
        public event EventHandler<Order> OnOrderDeliveryScheduled;

        public DeliveryService(int totalCars, string ServiceName, int speed)
        {
            this.TotalCars = totalCars;
            this.ServiceName = ServiceName;
            for (int i = 0; i < TotalCars; i++)
            {
                Vehicles.Add(new DeliveryServiceVehicle(speed));
            }
        }

        public void ReleaseVehicle(DeliveryServiceVehicle vehicle)
        {
            vehicle.Order = null;
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

        public void FinishDelivery(DeliveryServiceVehicle vehicle)
        {
            OnOrderDeliveryFinished?.Invoke(this, vehicle.Order);
            vehicle.CurrentDirection = DeliveryServiceVehicle.Direction.ToBase;
            vehicle.Distance = GetDistanceToBase(vehicle);
        }

        public int GetDistanceToBase(DeliveryServiceVehicle vehicle) => DISTANCE;
        public int GetDistanceToClient(DeliveryServiceVehicle vehicle) => DISTANCE;
        public int GetDistanceToShop(DeliveryServiceVehicle vehicle) => DISTANCE;

        public void RequestLateDelivery(Order order)
        {
            lock (DelayedDeliveries)
                DelayedDeliveries.Enqueue(order, 10);

            OnOrderDeliveryScheduled?.Invoke(this, order);
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
                OnOrderDeliveryStarted?.Invoke(this, order);
            }
            return true;
        }

        public void TakeGoodFromShop(DeliveryServiceVehicle vehicle)
        {
            if (vehicle.Order.Shop.GetOrderFromStock(vehicle.Order))
            {
                vehicle.CurrentDirection = DeliveryServiceVehicle.Direction.ToClient;
                vehicle.Distance = GetDistanceToClient(vehicle);
                OnOrderTaken?.Invoke(this, vehicle.Order);
            }
            else
               FinishDelivery(vehicle);
        }
    }
}
