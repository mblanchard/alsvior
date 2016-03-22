using Alsvior.Representations.Config;
using Alsvior.Representations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Interfaces
{
    public interface IWeatherClientWrapper
    {
        List<WeatherNode> GetNodes();
    }
}
