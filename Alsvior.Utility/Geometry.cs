
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility
{
    public static class Geometry
    {

        /// <summary>
        /// Calculate the distance between two coords (expressed as fixed-point values)
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>Distance in meters, expressed as a fixed-point value</returns>
        public static int Measure(int lat1, int lon1, int lat2, int lon2) //with fixed-point coversion
        {
            var floatingPointResult = Measure(
                FixedPointCoordConversion.ToDouble(lat1),
                FixedPointCoordConversion.ToDouble(lon1),
                FixedPointCoordConversion.ToDouble(lat2),
                FixedPointCoordConversion.ToDouble(lon2)
            );
            return FixedPointCoordConversion.ToInt(floatingPointResult);
        }

        /// <summary>
        /// Calculate the distance between two coords
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>Distance in meters</returns>
        public static double Measure(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6378.137; // Radius of earth in KM
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d * 1000; // meters 
        }
    }
}
