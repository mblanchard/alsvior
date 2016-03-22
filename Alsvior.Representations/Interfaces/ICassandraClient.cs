using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Interfaces
{
    public interface ICassandraClient
    {
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> filter = null, string keyspace = null) where T : class;
        void Insert<T>(List<T> records, string keyspace = null) where T : class;
    }
}
