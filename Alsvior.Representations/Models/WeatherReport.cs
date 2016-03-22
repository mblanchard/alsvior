using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Models
{
    public class WeatherReport
    {
        public List<WeatherDaily> Daily { get; set; } = new List<WeatherDaily>();
        public List<WeatherHourly> Hourly { get; set; } = new List<WeatherHourly>();
    }
}
