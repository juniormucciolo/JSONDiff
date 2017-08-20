using System.Threading.Tasks;
using JsonDiff.Models;

namespace JsonDiff.Repository
{
    public interface IRepository
    {
        Json GetById(string id);
        bool AddOrUpdate(Json obj);
        Task SaveJsonAsync(string id, string json, Side side);
        Task SaveAsync();
    }
}