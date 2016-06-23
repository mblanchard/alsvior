using Alsvior.Representations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Models
{
    public class InverterNode: ILocatable
    {
        #region Coord Properties
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        #endregion Coord Properties

        public string City { get; set; }
        public string State { get; set; }
        public string Zip_Short { get; set; }
        public string Utility { get; set; }
    }
}
