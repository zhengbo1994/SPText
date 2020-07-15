﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.EF.EF2
{
    public class ModelDbset : DbContext
    {
        private DbContext DBContext { get; set; }
        public ModelDbset() : base()
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


        public DbSet<Company> Company { get; set; }
        public DbSet<User> User { get; set; }
    }
}