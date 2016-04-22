using System.Collections.Generic;

namespace Alsvior.Representations.Config
{
    public class CassandraConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Keyspace { get; set; }
        public List<string> Nodes { get; set; }
    }
}
