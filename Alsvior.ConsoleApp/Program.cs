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
            PrintPrompt();

            string line;
            var date = DateTime.SpecifyKind(new DateTime(2015,6,21), DateTimeKind.Utc);
            while ((line = Console.ReadLine()) != null)
            {
                for(int i = 0; i < 650; i++)
                {
                    var result = _weatherClient.GetWeather(line,date);
                    _cassandraClient.Insert(result?.Hourly);
                    _cassandraClient.Insert(result?.Daily);

                    if (result?.Hourly != null && result.Hourly.Any() && result?.Daily != null && result.Daily.Any())
                    {
                        var message = String.Format("Added {0} hourly forecast(s) and {1} daily forecast for node {2} on date {3}", 
                            result.Hourly.Count, result.Daily.Count, line, date.ToShortDateString());
                        Console.WriteLine("Got Weather: " + date.ToShortDateString());
                        /*
                        var slackPost = _slackClient.PostMessage(message, "Sun");
                        if (slackPost)
                        {
                            Console.WriteLine("And I told Slack about it :)");
                        }
                        else {
                            Console.WriteLine("But I couldn't find Slack :(");
                        }
                        */
                    }
                    else
                    {
                        Console.WriteLine("I fell down :(");
                    }
                    date = date.AddDays(-1);
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
