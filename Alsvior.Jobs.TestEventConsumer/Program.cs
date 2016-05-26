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
        static EventHubConsumer _eventHubConsumer;
        static SlackClientWrapper _slackClient;

        static void Run()
        {

            InitSlackClient();
            InitEventConsumer();
            
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
            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
        }

        private static void InitSlackClient()
        {
            var config = SlackConfigSection.GetConfig();
            _slackClient = new SlackClientWrapper(config);
        }

        private static void InitEventConsumer()
        {
            var config = EventHubConfigSection.GetConfig();
            _eventHubConsumer = new EventHubConsumer(config, "weather");
            _eventHubConsumer.RegisterEventProcessor<SimpleEventProcessor>();
        }
    }

}
