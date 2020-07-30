using SPText.Common.DataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.DataHelper.Repository.IRepository
{
    public class RepositoryFactoryT<T> where T : class, new()
    {
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepositoryT<T> BaseRepository(string connString)
        {
            return new RepositoryT<T>(DbFactory.Base(connString, DatabaseType.SqlServer));
        }
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepositoryT<T> BaseRepository(string connString, DatabaseType type)
        {
            return new RepositoryT<T>(DbFactory.Base(connString, type));
        }
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IRepositoryT<T> BaseRepository(string connString, string type)
        {
            switch (type)
            {
                case "SqlServer":
                    return new RepositoryT<T>(DbFactory.Base(connString, DatabaseType.SqlServer));
                case "Oracle":
                    return new RepositoryT<T>(DbFactory.Base(connString, DatabaseType.Oracle));
                case "MySql":
                    return new RepositoryT<T>(DbFactory.Base(connString, DatabaseType.MySql));
                default:
                    return new RepositoryT<T>(DbFactory.Base(connString, DatabaseType.SqlServer));
            }
        }
        /// <summary>
        /// 定义仓储（基础库）
        /// </summary>
        /// <returns></returns>
        public IRepositoryT<T> BaseRepository()
        {
            return new RepositoryT<T>(DbFactory.Base());
        }

        /// <summary>
        /// 定义仓储（数据库）
        /// </summary>
        /// <returns></returns>
        public IRepositoryT<T> DataRepository()
        {
            return new RepositoryT<T>(DbFactory.Data());
        }
    }
}
