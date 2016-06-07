
namespace Alsvior.Representations.Interfaces
{
    public interface ICassandraClient
    {
        ICassandraSession CreateSession(string keyspace = null);
    }
}
