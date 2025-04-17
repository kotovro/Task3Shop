using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class DeliveryServiceModel
    {
        public int TotalCars { get; set; }
        public int AvailableCars { get; set; }
        public int CarsReadyForDelivery { get; set; }
        public string ServiceName { get; set; }

        DeliveryServiceModel(int totalCars, int availbaleCars, int carsReadyForDelivery, string ServiceName)
        {
            this.TotalCars = totalCars;
            this.AvailableCars = availbaleCars;
            this.CarsReadyForDelivery = carsReadyForDelivery;
            this.ServiceName = ServiceName;
        }
    }
}
