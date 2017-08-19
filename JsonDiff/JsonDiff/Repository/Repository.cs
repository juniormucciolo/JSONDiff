using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using JsonDiff.Models;

namespace JsonDiff.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext db;
        private readonly DbSet<Json> dbSet;

        public Repository()
        {
            db = new DatabaseContext();
            dbSet = db.Set<Json>();
        }

        public Json GetById(string id)
        {
            var result = db.Json.FirstOrDefault(x => x.JsonId == id);

            if (result == null)
            {
                return new Json();
            }

            return result;
        }

        public bool AddOrUpdate(Json obj)
        {
            try
            {
                dbSet.AddOrUpdate(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveJson(string id, string json, Side side)
        {
            var jsonById = GetById(id);

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
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }


    }
}