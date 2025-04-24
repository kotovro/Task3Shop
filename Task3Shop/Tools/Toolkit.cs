using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Shop.Tools
{
    public class Toolkit
    {
        public static List<T> GetShuffledList<T>(IEnumerable<T> list)
        {
            var random = new Random();
            return list.OrderBy(x => random.Next()).ToList();
        }
    }
}
