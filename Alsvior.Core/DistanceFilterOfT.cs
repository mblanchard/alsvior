using Alsvior.Representations.Interfaces;
using Alsvior.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Core
{
    public class DistanceFilter<T> where T : ILocatable
    {

        #region Properties
        private double _originLat;
        private double _originLon;
        private double _distanceThreshold;

        #endregion Properties
        public DistanceFilter(int lat, int lon, double distanceThreshold)
        {
            var doubleLat = FixedPointCoordConversion.ToDouble(lat);
            var doubleLon = FixedPointCoordConversion.ToDouble(lon);
            _originLat = doubleLat; _originLon = doubleLon; _distanceThreshold = distanceThreshold;
        }

        public DistanceFilter(double lat, double lon, double distanceThreshold)
        {
            _originLat = lat; _originLon = lon; _distanceThreshold = distanceThreshold;
        }

        public IEnumerable<T> FilterCoords(IEnumerable<T> coords)
        {
            return coords.Where(x => Geometry.Measure(
               _originLat,
               _originLon,
               FixedPointCoordConversion.ToDouble(x.Latitude),
               FixedPointCoordConversion.ToDouble(x.Longitude)) < _distanceThreshold
            );
        }
    }
}
