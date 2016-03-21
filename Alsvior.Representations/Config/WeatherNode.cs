using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Config
{
    public class WeatherNode
    {
        public string Name { get; set; }
        public int Latitude { get; set; } //Fixed Point
        public int Longitude { get; set; } //Fixed Point
    }
}
