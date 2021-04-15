using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.EF.EF2
{
    public class ModelDbset : DbContext
    {
        public DbContext DBContext { get; set; }
        public ModelDbset() : base("name=DataContext")
        {
            CreateDatabaseContext("DataContext");
        }

        public ModelDbset(string connection) : base(connection)
        {
            CreateDatabaseContext(connection);
        }

        public void CreateDatabaseContext(string connection)
        {
            DBContext = new DbContext(connection);
            DBContext.Configuration.ProxyCreationEnabled = false;
            DBContext.Configuration.LazyLoadingEnabled = false;
            DBContext.Configuration.ValidateOnSaveEnabled = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(DBContext);
        }


        public void Dispose(bool disposing)
        {
            if (disposing==true)
            {
                if (DBContext!=null)
                {
                    DBContext = null;
                }
            }
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<User> User { get; set; }
    }
}
