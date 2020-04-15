using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPText.EF
{
    public class Uow:IDisposable
    {
        private DatabaseContext DBContext { get; set; }

        public Uow(string connection) 
        {
            CreateDatabaseContext(connection);
        }

        public Uow()
        {
            CreateDatabaseContext("DataContext");
        }

        public void CreateDatabaseContext(string connection) {
            DBContext = new DatabaseContext(connection);
            DBContext.Configuration.ProxyCreationEnabled = false;
            DBContext.Configuration.LazyLoadingEnabled = false;
            DBContext.Configuration.ValidateOnSaveEnabled = false;
        }

        public void Commit() {
            DBContext.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(DBContext);
        }

        public  virtual void Dispose(bool dispose) {
            if (dispose)
            {
                if (DBContext!=null)
                {
                    DBContext.Dispose();
                }
            }
        }

        public void CreateTransactions(List<Uow> uows) {
            TransactionScope transactionScope = new TransactionScope();
            foreach (var uow in uows)
            {
                uow.Commit();
            }
            transactionScope.Complete();
        }

        private Repository<Company> _rCompany;
        public Repository<Company> Company
        {
            get
            {
                if (_rCompany == null)
                    _rCompany = new Repository<Company>(DBContext);
                return _rCompany;
            }
        }

        private Repository<User> _rUser;
        public Repository<User> User
        {
            get
            {
                if (_rUser == null)
                    _rUser = new Repository<User>(DBContext);
                return _rUser;
            }
        }
    }
}
