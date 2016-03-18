using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility.Config
{
    public class SlackConfig
    {
        public string WebhookURL { get; set; }
        public string Channel { get; set; }
        public string Username { get; set; }
        public List<SlackIcon> Icons { get; set; }
    }
}
