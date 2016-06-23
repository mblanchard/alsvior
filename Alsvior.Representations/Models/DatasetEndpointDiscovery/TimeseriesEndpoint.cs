using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Models
{
    public class TimeSeriesEndpoint
    {
        public TimeSeriesEndpoint() { }
        public TimeSeriesEndpoint(string name, string uri) { Name = name; URI = uri; }
        public string Name { get; set; }
        public string URI { get; set; }
    }
}
