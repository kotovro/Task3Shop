using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    class CustomerModel //add here reaction for out of stcok
    {
        List<OrderModel> orders = new List<OrderModel>();
        String Address { get;  }
        String Name { get;  }

        IOutOfStockClientStrategy clientStrategy;
        CustomerModel(IOutOfStockClientStrategy strategy)
        {
            clientStrategy = strategy;
        }
    }
}
