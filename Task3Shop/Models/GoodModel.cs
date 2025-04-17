using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Models
{
    public class GoodModel
    {
        public String Name { get; }
        
        public GoodModel(String name)
        {
            Name = name;
        }

        public GoodModel()
        {
        }
    }
}
