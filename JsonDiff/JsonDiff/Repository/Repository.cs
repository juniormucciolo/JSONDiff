using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using JsonDiff.Models;

namespace JsonDiff.Repository
{
    /// <summary>
    /// JSON Repository
    /// </summary>
    public class Repository : IRepository
    {
        private readonly DatabaseContext _db;
        private readonly DbSet<Json> _dbSet;

        /// <summary>
        /// JSON Repository Contructor.
        /// </summary>
        public Repository()
        {
            _db = new DatabaseContext();
            _dbSet = _db.Set<Json>();
        }

        /// <summary>
        /// Get JSON by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Json> GetByIdAsync(string id)
        {
            IQueryable<Json> query = _dbSet;
            var result = await (query.FirstOrDefaultAsync(x => x.JsonId == id));
            return result ?? new Json();
        }

        /// <summary>
        /// Add Or Update JSON asynchronously.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddOrUpdate(Json obj)
        {
            try
            {
                _dbSet.AddOrUpdate(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Save JSON asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public async Task SaveJsonAsync(string id, string json, Side side)
        {
            var jsonById = GetByIdAsync(id).Result;

            if (side == Side.Left)
            {
                jsonById.Left = json;
            }
            else
            {
                jsonById.Right = json;
            }

            jsonById.JsonId = id;

            AddOrUpdate(jsonById);

            await SaveAsync();
        }

        /// <summary>
        /// Perform dbset saving state asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}