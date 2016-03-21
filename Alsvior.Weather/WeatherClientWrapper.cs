using Alsvior.Representations;
using Alsvior.Representations.Config;
using Alsvior.Representations.Interfaces;
using Alsvior.Utility;
using ForecastIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsvior.Weather
{
    public class WeatherClientWrapper: IWeatherClientWrapper
    {
        #region Properties
        private WeatherConfig _config;
        #endregion Properties

        public WeatherClientWrapper(WeatherConfig config)
        {
            _config = config;
        }

        public WeatherReport GetWeather(string nodeName, DateTime? time = null)
        {
            var matchingNode = _config.Nodes.FirstOrDefault(x => x.Name == nodeName);
            if (matchingNode == null) return null;
            var fixedPointLat = matchingNode.Latitude;
            var fixedPointLon = matchingNode.Longitude;

            var lat = FixedPointCoordConversion.ToFloat(fixedPointLat);
            var lon = FixedPointCoordConversion.ToFloat(fixedPointLon);

            var request = time.HasValue ? new ForecastIORequest(_config.APIKey, lat, lon, time.Value, Unit.us) : new ForecastIORequest(_config.APIKey, lat, lon, Unit.us);
            var response = request.Get();
            var report = new WeatherReport();
            report.Hourly = response.hourly?.data?.Select(x => MapWeatherHourly(x, fixedPointLat, fixedPointLon)).ToList();
            report.Daily = response.daily?.data?.Select(x => MapWeatherDaily(x, fixedPointLat, fixedPointLon)).ToList();
            return report;
        }

        public List<WeatherNode> GetNodes()
        {
            return _config.Nodes;
        }

        private WeatherDaily MapWeatherDaily(DailyForecast daily, int latitude, int longitude)
        {
            return new WeatherDaily()
            {
                ApparentTemperatureMax = daily.apparentTemperatureMax,
                ApparentTemperatureMaxTime = daily.apparentTemperatureMaxTime,
                ApparentTemperatureMin = daily.apparentTemperatureMin,
                ApparentTemperatureMinTime = daily.apparentTemperatureMinTime,
                CloudCover = daily.cloudCover,
                DewPoint = daily.dewPoint,
                Humidity = daily.humidity,
                Icon = daily.icon,
                MoonPhase = daily.moonPhase,
                Ozone = daily.ozone,
                PrecipIntensity = daily.precipIntensity,
                PrecipIntensityMax = daily.precipIntensityMax,
                PrecipProbability = daily.precipProbability,
                Pressure = daily.pressure,
                Summary = daily.summary,
                SunriseTime = daily.sunriseTime,
                SunsetTime = daily.sunsetTime,
                TemperatureMax = daily.temperatureMax,
                TemperatureMaxTime = daily.temperatureMaxTime,
                TemperatureMin = daily.temperatureMin,
                TemperatureMinTime = daily.temperatureMinTime,
                Time = daily.time,
                Visibility = daily.visibility,
                WindBearing = daily.windBearing,
                WindSpeed = daily.windSpeed,

                Latitude = latitude,
                Longitude = longitude
            };
        }
        private WeatherHourly MapWeatherHourly(HourForecast daily, int latitude, int longitude)
        {
            return new WeatherHourly()
            {
                ApparentTemperature = daily.apparentTemperature,
                CloudCover = daily.cloudCover,
                DewPoint = daily.dewPoint,
                Humidity = daily.humidity,
                Icon = daily.icon,
                Ozone = daily.ozone,
                PrecipIntensity = daily.precipIntensity,
                PrecipProbability = daily.precipProbability,
                PrecipType = daily.precipType,
                Pressure = daily.pressure,
                Summary = daily.summary,
                Temperature = daily.temperature,
                Time = daily.time,
                Visibility = daily.visibility,
                WindBearing = daily.windBearing,
                WindSpeed = daily.windSpeed,

                Latitude = latitude,
                Longitude = longitude
            };
        }

    }
}
