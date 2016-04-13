using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Config
{
    public class EventNamespaceConfig
    {
        public string Namespace { get; set; }
        public List<EventHubConfig> Hubs { get; set; }
    }
    public class EventHubConfig
    {
        public EventHubConfig(EventHubElement configElement) { Name = configElement.Name; SendPolicy = configElement.SendPolicy; ListenPolicy = configElement.ListenPolicy; }
        public string Name { get; set; }
        public string SendPolicy { get; set; }
        public string ListenPolicy { get; set; }
    }
}
