using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class DeliveryServiceAutomobileModel
    {
        int Speed { get;  }
        Order order { get;  }
        DeliveryServiceAutomobileModel(int speed, Order
            order)
        {
            Speed = speed;
            this.order = order;
        }
    }
}
