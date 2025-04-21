using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class DeliveryServiceVehicle
    {
        int Speed { get;  }
        Order Order { get; set; }


        //Direc
        DeliveryServiceVehicle(int speed, Order
            order)
        {
            this.Speed = speed;
            this.Order = order;
        }
    }
}
