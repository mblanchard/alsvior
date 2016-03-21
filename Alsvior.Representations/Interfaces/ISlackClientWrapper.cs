using System.Threading.Tasks;

namespace Alsvior.Representations.Interfaces
{
    public interface ISlackClientWrapper
    {
        Task<bool> PostMessageAsync(string message, string iconName);
        bool PostMessage(string message, string iconName);
    }
}
