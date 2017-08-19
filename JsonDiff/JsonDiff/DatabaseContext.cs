using System.Configuration;
using System.Data.Entity;
using JsonDiff.Models;

namespace JsonDiff
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("JsonDiff")
        {
            var connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<Json> Json { get; set; }
    }
}
