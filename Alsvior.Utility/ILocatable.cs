using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility.Interfaces
{
    public interface ILocatable
    {
        int Latitude { get; set; }
        int Longitude { get; set; }
    }
}
