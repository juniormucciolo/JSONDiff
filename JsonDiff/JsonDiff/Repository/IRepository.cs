using JsonDiff.Models;

namespace JsonDiff.Repository
{
    public interface IRepository
    {
        Json GetById(string id);
        bool AddOrUpdate(Json obj);
        void SaveJson(string id, string json, Side side);
        void Save();
    }
}