﻿using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Config;
using Cassandra;
using Cassandra.Data.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alsvior.DAL
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

        public IEnumerable<T> Get<T>(Expression<Func<T,bool>> filter = null, string keyspace = null) where T : class

        {
            using(var session = _cluster.Connect(keyspace ?? _keyspace))
            {
                var table = new Table<T>(session);
                if (filter != null)
                {
                    return table.Where(filter).Execute();
                }
                else return table.Execute();
            }
           
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> filter = null, string keyspace = null)
        {
            using (var session = _cluster.Connect(keyspace ?? _keyspace))
            {
                var table = new Table<T>(session);
                if (filter != null)
                {
                    return await table.Where(filter).ExecuteAsync();
                }
                else return await table.ExecuteAsync();
            }
        }

        public void Insert<T>(List<T> records, string keyspace = null) where T : class
        {
            using (var session = _cluster.Connect(keyspace ?? _keyspace))
            {
                var table = new Table<T>(session);
                var insertQueries = records.Select(x => table.Insert(x).ExecuteAsync()).ToList();
                Task.WaitAll(insertQueries.ToArray());
                return;
            }
        }
    }
}
