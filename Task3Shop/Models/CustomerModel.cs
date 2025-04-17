using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class CustomerModel //add here reaction for out of stcok
    {
        List<OrderModel> orders = new List<OrderModel>();
        public String Address { get;  }
        public String Name { get;  }

        IOutOfStockClientStrategy clientStrategy { get; set;  }
        public CustomerModel(IOutOfStockClientStrategy strategy)
        {
            clientStrategy = strategy;
        }

        public CustomerModel(string Name, string Address)
        {
            this.Name = Name;
            this.Address = Address;
        }
    }
}
