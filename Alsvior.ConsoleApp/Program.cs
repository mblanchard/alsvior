using Alsvior.DAL;
using Alsvior.Weather;
using Alsvior.Utility.Config;
using System;
using System.Linq;
using Alsvior.Utility.Slack;

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
            PrintPrompt();

            string line;
            while ((line = Console.ReadLine()) != null)
            {
                for(int i = 0; i < 650; i++)
                {
                    var date = DateTime.Now.AddDays(i * -2);
                    var result = _weatherClient.GetWeather(line,date);
                    _cassandraClient.Insert(result?.Hourly);
                    _cassandraClient.Insert(result?.Daily);

                    if (result?.Hourly != null && result.Hourly.Any() && result?.Daily != null && result.Daily.Any())
                    {
                        var message = String.Format("Added {0} hourly forecast(s) and {1} daily forecast(s) for date {2}", result.Hourly.Count, result.Daily.Count, date.ToShortDateString());
                        Console.WriteLine("Got Weather");
                        var slackPost = _slackClient.PostMessage(message, "Sun");
                        if (slackPost)
                        {
                            Console.WriteLine("And I told Slack about it :)");
                        }
                        else {
                            Console.WriteLine("But I couldn't find Slack :(");
                        }
                    }
                    else
                    {
                        Console.WriteLine("I fell down :(");
                    }
                }
               
                    
                
                PrintPrompt();
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
