using Alsvior.DAL;
using Alsvior.Weather;
using Alsvior.Representations.Config;
using System;
using System.Linq;
using Alsvior.Representations.Slack;

namespace Alsvior.ConsoleApp
{
    class Program
    {
        static CassandraClient _cassandraClient;
        static WeatherClientWrapper _weatherClient;
        static SlackClientWrapper _slackClient;

        static void PrintPrompt()
        {
            Console.WriteLine("Provide the weather node name");
        }


        static void Run()
        {
            var date = DateTime.SpecifyKind(new DateTime(2016,2,26), DateTimeKind.Utc);

            for(int i = 0; i < 650; i++)
            {
                var results = _weatherClient.GetWeatherForAllNodes(date);
                if (results == null) continue;
                int hourlyCount = 0, dailyCount = 0, nodeCount = 0;
                foreach (var result in results)
                {

                    if (result?.Hourly != null && result.Hourly.Any() && result?.Daily != null && result.Daily.Any())
                    {
                        hourlyCount += result.Hourly.Count;
                        dailyCount += result.Daily.Count;
                        nodeCount++;
                    }

                    _cassandraClient.Insert(result?.Hourly);
                    _cassandraClient.Insert(result?.Daily);
                }
                var message = String.Format("Added {0} hourly forecast(s) and {1} daily forecast for {2} nodes on {3}", hourlyCount, dailyCount, nodeCount, date.ToShortDateString());
                Console.WriteLine(message);
                date = date.AddDays(-1);
            }                 
        }


        static void Main(string[] args)
        {
            InitCassandraClient();
            InitSlackClient();
            InitWeatherClient();
            Run();
        }

        private static void InitCassandraClient()
        {
            var config = CassandraConfigSection.GetConfig();
            _cassandraClient = new CassandraClient(config);        
        }

        private static void InitWeatherClient()
        {
            var config = WeatherConfigSection.GetConfig();
            _weatherClient = new WeatherClientWrapper(config);
        }

        private static void InitSlackClient()
        {
            var config = SlackConfigSection.GetConfig();
            _slackClient = new SlackClientWrapper(config);
            
        }
    }
}
