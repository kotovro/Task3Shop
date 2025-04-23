using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class DeliveryServiceVehicle
    {
        public enum Direction 
        {
            ToBase = 0,
            ToShop = 1,
            ToClient = 2,
        };

        public Direction CurrentDirection { get; set; } = Direction.ToBase;
        public int Distance { get; set; } 
        public int Speed { get; private set; }
        public Order Order { get; set; }


        public DeliveryServiceVehicle(int speed)
        {
            this.Speed = speed;
        }
    }
}
