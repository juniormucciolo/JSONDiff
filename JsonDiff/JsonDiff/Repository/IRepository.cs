using System.Threading.Tasks;
using JsonDiff.Models;

namespace JsonDiff.Repository
{
    /// <summary>
    /// Repository Inteface.
    /// </summary>
    public interface IRepository
    {
        Task<Json> GetByIdAsync(string id);
        bool AddOrUpdate(Json obj);
        Task SaveJsonAsync(string id, string json, Side side);
        Task SaveAsync();
    }
}