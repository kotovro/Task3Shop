using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task3Shop.Tools;

namespace Task3Shop.Models
{
    public class Simulation
    {

        public event EventHandler<int> SimulationStepComplete;
        public event EventHandler<bool> SimulationFinished;

        private async Task<bool> ServeCustomer(Customer customer, IEnumerable<Good> goods, IEnumerable<Shop> shops) 
        {
            Random random = new Random();
            if (random.Next(10) != 1)
                return false;

            List<Good> shuffledGoods = Toolkit.GetShuffledList(goods);
            Good good = shuffledGoods.ElementAt(random.Next(shuffledGoods.Count));

            List<Shop> shuffledShops = Toolkit.GetShuffledList(shops);
            foreach (Shop shop in shuffledShops)
            {
                if (shop.IsGoodAvailable(good))
                {
                    customer.MakeOrder(good, shop);
                    break;
                }
            }
            return true;
        }

        private async Task<bool> ServeVehicle(DeliveryServiceVehicle vehicle, DeliveryService deliveryService)
        {
            if (vehicle.Distance == 0)
            {
                if (vehicle.CurrentDirection == DeliveryServiceVehicle.Direction.ToShop)
                {
                    deliveryService.TakeGoodFromShop(vehicle);
                }
                else if (vehicle.CurrentDirection == DeliveryServiceVehicle.Direction.ToClient)
                {
                    deliveryService.FinishDelivery(vehicle);
                }
            }
            else
                vehicle.Distance -= vehicle.Speed;

            return true;
        }

        public async Task<bool> DoSimulation(IEnumerable<Shop> shops, IEnumerable<Customer> customers, IEnumerable<DeliveryService> deliveryServices, IEnumerable<Good> goods, CancellationToken ct)
        {
            int counter = 0;
            while (shops.Any(s => s.Stock.Any(kv => kv.Value > 0)) && !ct.IsCancellationRequested)
            {
                List<Task<bool>> tasks = new();

                foreach (DeliveryService deliveryService in deliveryServices)
                {
                    foreach (DeliveryServiceVehicle vehicle in deliveryService.Vehicles)
                    {
                        tasks.Add(ServeVehicle(vehicle, deliveryService));
                    }
                }
                
                foreach (Customer cst in customers)
                {
                    tasks.Add(ServeCustomer(cst, goods, shops));
                }

                Task.WaitAll(tasks);

                SimulationStepComplete?.Invoke(this, counter++);
                await Task.Delay(300, ct);
            }
            SimulationFinished?.Invoke(this, ct.IsCancellationRequested);
            return true;
        }
    }


}
