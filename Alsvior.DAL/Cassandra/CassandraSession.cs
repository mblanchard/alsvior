using Alsvior.Representations.Interfaces;
using Cassandra;
using Cassandra.Data.Linq;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Alsvior.DAL.Cassandra
{
    public class CassandraSession: ICassandraSession, IDisposable
    {
        #region Properties
        //IDisposable
        private bool disposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        //Cassandra Session
        ISession _session;
        #endregion Properties

        #region Constructor
        public CassandraSession(ISession session)
        {
            _session = session;
        }
        #endregion Constructor

        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> filter = null, string keyspace = null) where T : class
        {
            var table = new Table<T>(_session);
            if (filter != null)
            {
                return table.Where(filter).Execute();
            }
            else return table.Execute();          
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> filter = null, string keyspace = null) where T : class
        {
            var table = new Table<T>(_session);
            if (filter != null)
            {
                return await table.Where(filter).ExecuteAsync();
            }
            else return await table.ExecuteAsync();
            
        }

        public void Insert<T>(List<T> records, string keyspace = null) where T : class
        {

            var table = new Table<T>(_session);
            var insertQueries = records.Select(x => table.Insert(x).ExecuteAsync()).ToList();
            Task.WaitAll(insertQueries.ToArray());
            return;
            
        }

        #region Dispose
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                _session.Dispose();
            }
            disposed = true;
        }
        #endregion Dispose
    }
}
