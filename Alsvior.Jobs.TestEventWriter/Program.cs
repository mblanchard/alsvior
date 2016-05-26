using Alsvior.DAL;
using Alsvior.Representations.Config;
using Alsvior.Representations.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Jobs.TestEventWriter
{
    class Program
    {
        const int NUM_TEST_EVENTS = 10;
        static EventHubProducer _eventHubProducer;
        static SlackClientWrapper _slackClient;

       

        static void Run()
        {

            InitSlackClient();
            InitEventProducer();
            WriteTestEvents();
        }

        static void WriteTestEvents()
        {
            var tasks = new Task[NUM_TEST_EVENTS];
            for(int i = 0; i < NUM_TEST_EVENTS; i++)
            {
                var msg = String.Format("Mock event #{0} created at {1}", i, DateTime.Now);
                tasks[i] = (_eventHubProducer.SendAsync(Encoding.ASCII.GetBytes(msg)));
            }
            Task.WaitAll(tasks);
            Console.WriteLine("Events Created");
        }

        static void LogToSlack()
        {
            var message = "TEST";
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

        private static void InitSlackClient()
        {
            var config = SlackConfigSection.GetConfig();
            _slackClient = new SlackClientWrapper(config);
        }

        private static void InitEventProducer()
        {
            var config = EventHubConfigSection.GetConfig();
            _eventHubProducer = new EventHubProducer(config, "weather");
            
        }
    }

}
