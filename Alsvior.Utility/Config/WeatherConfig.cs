using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility.Config
{
    public class WeatherConfig
    {
        public string APIKey { get; set; }
        public List<WeatherNode> Nodes { get; set; }
    }
}
