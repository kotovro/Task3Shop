using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class Simulation
    {
        public static async void DoSimulation(IEnumerable<Shop> shops, IEnumerable<Customer> customers, IEnumerable<DeliveryService> deliveryServices, IEnumerable<Good> goods)
        {

            Random random = new Random();
            while (true)
            {
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
                await Task.Delay(300);
            }
        }
    }
}
