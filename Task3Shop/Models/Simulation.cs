using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class Simulation
    {
        public event EventHandler<int> SimulationStepComplete;
        public async Task<bool> DoSimulation(IEnumerable<Shop> shops, IEnumerable<Customer> customers, IEnumerable<DeliveryService> deliveryServices, IEnumerable<Good> goods, CancellationToken ct)
        {

            Random random = new Random();
            int counter = 0;
            while (shops.Any(s => s.Stock.Any(kv => kv.Value > 0)) || !ct.IsCancellationRequested)
            {
                foreach (DeliveryService deliveryService in deliveryServices)
                {
                    foreach (DeliveryServiceVehicle vehicle in deliveryService.Vehicles)
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
                    }
                }
                foreach (Customer cst in customers)
                {
                    if (random.Next(10) != 1)
                        continue;

                    List<Good> shuffledGoods = Toolkit.GetShuffledList(goods);
                    Good good = shuffledGoods.ElementAt(random.Next(shuffledGoods.Count));

                    List<Shop> shuffledShops = Toolkit.GetShuffledList(shops);
                    foreach (Shop shop in shuffledShops)
                    {
                        if (shop.IsGoodAvailable(good))
                        {
                            cst.MakeOrder(good, shop);
                            break;
                        }
                    }
                }
                
                SimulationStepComplete.Invoke(this, counter++);
                await Task.Delay(300);
            }
            return true;
        }
    }
}
