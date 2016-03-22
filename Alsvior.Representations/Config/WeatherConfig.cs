using Alsvior.Representations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Config
{
    public class WeatherConfig
    {
        public string APIKey { get; set; }
        public List<WeatherNode> Nodes { get; set; }
    }
}
