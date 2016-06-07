using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Config;
using Cassandra;
using System;

namespace Alsvior.DAL.Cassandra
{
    public class CassandraClient: ICassandraClient
    {
        #region Properties
        private Cluster _cluster;
        private CassandraConfig _config;
        private string _keyspace;
        
        #endregion Properties


        #region Constructor
        public CassandraClient(CassandraConfig config)
        {
            _config = config;
            _cluster = Cluster.Builder()
                .WithCredentials(_config.Username, _config.Password)
                .AddContactPoints(_config.Nodes)
                .Build();

            _keyspace = _config.Keyspace;
        }
        #endregion Constructor


        #region Public Methods
        public ICassandraSession CreateSession(string keyspace = null)
        {
            return new CassandraSession(_cluster.Connect(keyspace ?? _config.Keyspace));
        }
        #endregion Public Methods
    }
}
