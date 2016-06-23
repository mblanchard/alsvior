using Alsvior.Representations.Interfaces;
using Alsvior.Utility;

namespace Alsvior.Representations.Models
{
    [AssociatedGeospatialModel(typeof(WeatherNode))]
    public class WeatherHourly : ILocatable, IChronometricable
    {
        #region Coord Properties
        public int Latitude { get; set; } //Fixed-point
        public int Longitude { get; set; } //Fixed-point
        #endregion Coord Properties

        #region Time 
        public long Time { get; set; }
        #endregion Time

        #region Weather Properties
        public float ApparentTemperature { get; set; }
        public float CloudCover { get; set; }
        public float DewPoint { get; set; }
        public float Humidity { get; set; }
        public string Icon { get; set; }
        public float Ozone { get; set; }
        public float PrecipIntensity { get; set; }
        public float PrecipProbability { get; set; }
        public string PrecipType { get; set; }
        public float Pressure { get; set; }
        public string Summary { get; set; }
        public float Temperature{ get; set; }
        
        public float Visibility { get; set; }
        public float WindBearing { get; set; }
        public float WindSpeed { get; set; }
        #endregion Weather Properties
    }
}
