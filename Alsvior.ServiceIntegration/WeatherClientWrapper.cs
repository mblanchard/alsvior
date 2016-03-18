using Alsvior.Representations;
using ForecastIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.ServiceIntegration
{
    public class WeatherClientWrapper
    {
        #region Properties
        public string APIKey { get; set; }
        #endregion Properties

        public WeatherClientWrapper(string apiKey)
        {
            APIKey = apiKey;
        }

        public WeatherReport GetWeather(float lat, float lon, DateTime? time = null)
        {
            var request = time.HasValue? new ForecastIORequest(APIKey, lat, lon, time.Value,Unit.us):new ForecastIORequest(APIKey, lat, lon, Unit.us);
            var response = request.Get();
            var report = new WeatherReport();
            report.Hourly = response.hourly?.data?.Select(x => MapWeatherHourly(x,lat, lon)).ToList();
            report.Daily = response.daily?.data?.Select(x => MapWeatherDaily(x, lat, lon)).ToList();
            return report;
        }

        private WeatherDaily MapWeatherDaily(DailyForecast daily, double latitude, double longitude)
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
        private WeatherHourly MapWeatherHourly(HourForecast daily, double latitude, double longitude)
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
