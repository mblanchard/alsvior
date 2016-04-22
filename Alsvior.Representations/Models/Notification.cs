using Alsvior.Representations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Models
{
    public class Notification: ILocatable
    {
        #region Coord Properties
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        #endregion Coord Properties

        public long Timestamp { get; set; }
        public string Description { get; set; }

        public Notification(int lat, int lon, long time, string desc)
        {
            Latitude = lat; Longitude = lon; Timestamp = time; Description = desc;
        }
        public override string ToString()
        {
            return String.Format("{0}_{1}_{2}_{3}", Latitude, Longitude, Timestamp, Description);   
        }
    }
}
