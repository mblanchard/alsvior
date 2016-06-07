using Alsvior.DAL;
using Alsvior.DAL.Cassandra;
using Alsvior.Representations.Config;
using Alsvior.Representations.Interfaces;
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
        static ICassandraSession _session;
        static WeatherClientWrapper _weatherClient;
        static SlackClientWrapper _slackClient;
        static int DEFAULT_DAYS_TO_CHECK_FOR_HISTORICAL_WEATHER = 365;
        static int MAX_HISTORICAL_FORECASTIO_REQUESTS_PER_RUN = 25;

        static List<DateTime> GetMissingWeatherDatesForNode(WeatherNode node, DateTime startDate, DateTime endDate)
        {
            var latitude = node.Latitude;
            var longitude = node.Longitude;

            var dailyReports = _session.Get<WeatherDaily>(x => x.Latitude == latitude && x.Longitude == longitude).ToList();


            var missingDates = new List<DateTime>();
            var isWesternHemisphere = node.Longitude <= 0;
            var tzOffsetMin = isWesternHemisphere ? -2 : -14;
            var tzOffsetMax = isWesternHemisphere ? 14 : 2;
            //Or "when is 12:00 AM at this location, relative to UTC?"

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {

                var timestamp = TimeConversion.ConvertToTimestamp(date);
                var tzOffsetMinTime = TimeConversion.ConvertToTimestamp(date.AddHours(tzOffsetMin));
                var tzOffsetMaxTime = TimeConversion.ConvertToTimestamp(date.AddHours(tzOffsetMax));
                if (!dailyReports.Any(x => x.Time >= tzOffsetMinTime && x.Time <= tzOffsetMaxTime))
                {
                    var tzOffsetForDate = TimeConversion.GetTimezoneOffsetFromLocationAndDateInSeconds(timestamp, node.Latitude, node.Longitude);
                    missingDates.Add(date.AddSeconds(-tzOffsetForDate));
                }
            }
            return missingDates;
        }

        static void Run()
        {
            InitCassandraClient();
            InitSlackClient();
            var weatherNodes = _session.Get<WeatherNode>().ToList();
            InitWeatherClient(weatherNodes);

            UpdateRecentWeather(weatherNodes);
            UpdateHistoricalWeather(weatherNodes);
        }

        private static void UpdateHistoricalWeather(List<WeatherNode> nodes)
        {
            var endDate = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
            var startDate = endDate.AddDays(-1 * DEFAULT_DAYS_TO_CHECK_FOR_HISTORICAL_WEATHER);
            int requestCount = 0;

            foreach (var node in nodes)
            {
                int nodeRequests = 0;
                var missingDates = GetMissingWeatherDatesForNode(node, startDate, endDate);
                foreach (var date in missingDates)
                {
                    var result = _weatherClient.GetWeather(node.Name, date);
                    

                    if (result?.Hourly != null && result.Hourly.Any() && result?.Daily != null && result.Daily.Any())
                    {
                        var dateTime = TimeConversion.ConvertToDateTime(result.Daily[0].Time);

                        //Write to C*
                        _session.Insert(result.Hourly);
                        _session.Insert(result.Daily);

                        //Increment counters
                        nodeRequests++;
                        requestCount++;

                        //Check request limits and exit if reached cap
                        if (requestCount >= MAX_HISTORICAL_FORECASTIO_REQUESTS_PER_RUN)
                        {
                            LogNodeHistoricalUpdate(nodeRequests, node.Name, missingDates.First(), date);
                            return;
                        }
                    }
                }
                if(nodeRequests > 0)
                {
                    LogNodeHistoricalUpdate(nodeRequests, node.Name, missingDates.First(), missingDates.Last());
                    return;
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
                    _session.Insert(result.Hourly);
                    _session.Insert(result.Daily);

                    //Increment counters
                    hourlyCount += result.Hourly.Count;
                    dailyCount += result.Daily.Count;
                    requestCount++;
                }
            }
            LogRecentUpdate(hourlyCount, dailyCount, nodes.Count, requestCount);
        }

        static void LogNodeHistoricalUpdate(int requestCount, string nodeName, DateTime startDate, DateTime endDate)
        {
            var message = String.Format("Added {0} missing days for node {1} between {2} and {3}", requestCount, nodeName, startDate.ToShortDateString(), endDate.ToShortDateString());
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
            _session = _cassandraClient.CreateSession();
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
