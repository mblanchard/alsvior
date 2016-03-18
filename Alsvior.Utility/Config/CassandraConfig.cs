using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility.Config
{
    public class CassandraConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Keyspace { get; set; }
        public List<string> Nodes { get; set; }
    }
}
