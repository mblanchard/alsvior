﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations
{
    public class WeatherDaily
    {
        #region Coord Properties
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        #endregion Coord Properties

        #region Weather Properties
        public float ApparentTemperatureMax { get; set; }
        public long ApparentTemperatureMaxTime { get; set; }
        public float ApparentTemperatureMin { get; set; }
        public long ApparentTemperatureMinTime { get; set; }
        public float CloudCover { get; set; }
        public float DewPoint { get; set; }
        public float Humidity { get; set; }
        public string Icon { get; set; }
        public float MoonPhase { get; set; }
        public float Ozone { get; set; }
        public float PrecipIntensity { get; set; }
        public float PrecipIntensityMax { get; set; }
        public float PrecipProbability { get; set; }
        public float Pressure { get; set; }
        public string Summary { get; set; }
        public long SunriseTime { get; set; }
        public long SunsetTime { get; set; }
        public float TemperatureMax { get; set; }
        public long TemperatureMaxTime { get; set; }
        public float TemperatureMin { get; set; }
        public long TemperatureMinTime { get; set; }
        public long Time { get; set; }
        public float Visibility { get; set; }
        public float WindBearing { get; set; }
        public float WindSpeed { get; set; }
        #endregion Weather Properties
    }
}
