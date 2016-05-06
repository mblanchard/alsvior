using Alsvior.DAL;
using Alsvior.Representations.Config;
using Alsvior.Representations.Models;
using Alsvior.Representations.Slack;
using Alsvior.Utility;
using Alsvior.Weather;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsvior.Jobs.HistoricalWeather
{
    class Program
    {
        static CassandraClient _cassandraClient;
        static WeatherClientWrapper _weatherClient;
        static SlackClientWrapper _slackClient;
        static int DEFAULT_DAYS_TO_CHECK_FOR_HISTORICAL_WEATHER = 365;
        static int MAX_HISTORICAL_FORECASTIO_REQUESTS_PER_RUN = 25;

        static List<DateTime> GetMissingWeatherDatesForNode(WeatherNode node, DateTime startDate, DateTime endDate)
        {
            var latitude = node.Latitude;
            var longitude = node.Longitude;
            var dailyReports = _cassandraClient.Get<WeatherDaily>(x => x.Latitude == latitude && x.Longitude == longitude).ToList();

            var missingDates = new List<DateTime>();
            var isWesternHemisphere = node.Longitude <= 0;
            var tzOffsetMin = isWesternHemisphere ? -1 : -14;
            var tzOffsetMax = isWesternHemisphere ? 14 : 1;
            //Or "when is 12:00 AM at this location, relative to UTC?"

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var tzOffsetMinTime = TimeConversion.ConvertToTimestamp(date.AddHours(tzOffsetMin));
                var tzOffsetMaxTime = TimeConversion.ConvertToTimestamp(date.AddHours(tzOffsetMax));
                if (!dailyReports.Any(x => x.Time >= tzOffsetMinTime && x.Time <= tzOffsetMaxTime))
                {
                    missingDates.Add(isWesternHemisphere ? date.AddDays(1) : date);
                }
            }
            return missingDates;
        }

        static void Run()
        {
            InitCassandraClient();
            InitSlackClient();
            var weatherNodes = _cassandraClient.Get<WeatherNode>().ToList();
            InitWeatherClient(weatherNodes);

            UpdateRecentWeather(weatherNodes);
            UpdateHistoricalWeather(weatherNodes);
        }

        private static void UpdateHistoricalWeather(List<WeatherNode> nodes)
        {
            var endDate = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
            var startDate = endDate.AddDays(-1 * DEFAULT_DAYS_TO_CHECK_FOR_HISTORICAL_WEATHER);
            int hourlyCount = 0, dailyCount = 0, requestCount = 0, nodeCount = 0;

            foreach (var node in nodes)
            {
                var missingDates = GetMissingWeatherDatesForNode(node, startDate, endDate);
                if(missingDates.Any()) { nodeCount++; }
                foreach (var date in missingDates)
                {
                    var result = _weatherClient.GetWeather(node.Name, date);

                    if (result?.Hourly != null && result.Hourly.Any() && result?.Daily != null && result.Daily.Any())
                    {
                        //Write to C*
                        _cassandraClient.Insert(result.Hourly);
                        _cassandraClient.Insert(result.Daily);

                        //Increment counters
                        hourlyCount += result.Hourly.Count;
                        dailyCount += result.Daily.Count;
                        requestCount++;

                        //Check request limits and exit if reached cap
                        if (requestCount >= MAX_HISTORICAL_FORECASTIO_REQUESTS_PER_RUN)
                        {
                            Log(hourlyCount, dailyCount, nodeCount, startDate, endDate, requestCount);
                            return;
                        }
                    }
                }
                if(requestCount > 0)
                {
                    Log(hourlyCount, dailyCount, nodeCount, startDate, endDate, requestCount);
                }
            }
        }

        private static void UpdateRecentWeather(List<WeatherNode> nodes)
        {
            int hourlyCount = 0, dailyCount = 0, requestCount = 0;
            foreach (var node in nodes)
            {
                var result = _weatherClient.GetWeather(node.Name);
                if (result?.Hourly != null && result.Hourly.Any() && result?.Daily != null && result.Daily.Any())
                {
                    //Write to C*
                    _cassandraClient.Insert(result.Hourly);
                    _cassandraClient.Insert(result.Daily);

                    //Increment counters
                    hourlyCount += result.Hourly.Count;
                    dailyCount += result.Daily.Count;
                    requestCount++;
                }
            }
            LogRecentUpdate(hourlyCount, dailyCount, nodes.Count, requestCount);
        }

        static void Log(int hourlyCount, int dailyCount, int nodeCount, DateTime startDate, DateTime endDate, int requestCount)
        {
            var message = String.Format("Added {0} hourly forecast(s) and {1} daily forecast(s) for {2} weather nodes between {3} and {4} ({5} requests to forecast.io)", hourlyCount, dailyCount, nodeCount, startDate.ToShortDateString(), endDate.ToShortDateString(), requestCount);
            _slackClient.PostMessage(message, "Sun");
            Console.WriteLine(message);
        }

        static void LogRecentUpdate(int hourlyCount, int dailyCount, int nodeCount, int requestCount)
        {
            var message = String.Format("Added {0} updated hourly forecast(s) and {1} daily forecast(s) for {2} weather nodes ({3} requests to forecast.io)", hourlyCount, dailyCount, nodeCount, requestCount);
            Console.WriteLine(message);
        }

        static void Main(string[] args)
        {
            Run();
        }

        private static void InitCassandraClient()
        {
            var config = CassandraConfigSection.GetConfig();
            _cassandraClient = new CassandraClient(config);
        }

        private static void InitWeatherClient(List<WeatherNode> nodes)
        {
            var config = WeatherConfigSection.GetConfig();
            if (nodes != null && nodes.Any())
            {
                config.Nodes = nodes;
            }
            _weatherClient = new WeatherClientWrapper(config);
        }

        private static void InitSlackClient()
        {
            var config = SlackConfigSection.GetConfig();
            _slackClient = new SlackClientWrapper(config);

        }
    }
}
