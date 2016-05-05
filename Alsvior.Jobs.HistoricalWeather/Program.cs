using Alsvior.DAL;
using Alsvior.Representations.Config;
using Alsvior.Representations.Models;
using Alsvior.Representations.Slack;
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
        static int DEFAULT_DAYS_TO_CHECK_FOR_HISTORICAL_WEATHER = 370;

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
                var tzOffsetMinTime = ConvertToTimestamp(date.AddHours(tzOffsetMin));
                var tzOffsetMaxTime = ConvertToTimestamp(date.AddHours(tzOffsetMax));
                if (!dailyReports.Any(x => x.Time >= tzOffsetMinTime && x.Time <= tzOffsetMaxTime))
                {
                    missingDates.Add(isWesternHemisphere ? date.AddDays(1) : date);
                }
            }
            return missingDates;
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        static void Run()
        {
            var endDate = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
            var startDate = endDate.AddDays(-1 * DEFAULT_DAYS_TO_CHECK_FOR_HISTORICAL_WEATHER);

            InitCassandraClient();
            InitSlackClient();
            var weatherNodes = _cassandraClient.Get<WeatherNode>().ToList();
            InitWeatherClient(weatherNodes);

            int hourlyCount = 0, dailyCount = 0, nodeCount = 0;

            foreach (var node in weatherNodes)
            {
                var missingDates = GetMissingWeatherDatesForNode(node, startDate, endDate);
                foreach (var date in missingDates)
                {
                    var result = _weatherClient.GetWeather(node.Name, date);
                    if (result == null)
                    {
                        continue;
                    }
                    if (result.Hourly != null && result.Hourly.Any() && result.Daily != null && result.Daily.Any())
                    {
                        hourlyCount += result.Hourly.Count;
                        dailyCount += result.Daily.Count;
                        nodeCount++;
                    }

                    _cassandraClient.Insert(result.Hourly);
                    _cassandraClient.Insert(result.Daily);
                }
            }

            var message = String.Format("Added {0} hourly forecast(s) and {1} daily forecast(s) for {2} nodes between {3} and {4}", hourlyCount, dailyCount, weatherNodes.Count, startDate, endDate);
            _slackClient.PostMessage(message, "Sun");
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
