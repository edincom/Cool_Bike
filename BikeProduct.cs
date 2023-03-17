using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice_bike
{
    internal class BikeProduct
    {
        public string Model { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; } = 1;
    }

}
