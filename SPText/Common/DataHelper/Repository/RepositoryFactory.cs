using SPText.Common.DataHelper;
using SPText.Common.DataHelper.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.DataHelper.Repository
{
    public class RepositoryFactory
    {
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepository BaseRepository(string connString)
        {
            return new Repository(DbFactory.Base(connString, DatabaseType.SqlServer));
        }
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepository BaseRepository(string connString, DatabaseType type)
        {
            return new Repository(DbFactory.Base(connString, type));
        }
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IRepository BaseRepository(string connString, string type)
        {
            switch (type)
            {
                case "SqlServer":
                    return new Repository(DbFactory.Base(connString, DatabaseType.SqlServer));
                case "Oracle":
                    return new Repository(DbFactory.Base(connString, DatabaseType.Oracle));
                case "MySql":
                    return new Repository(DbFactory.Base(connString, DatabaseType.MySql));
                default:
                    return new Repository(DbFactory.Base(connString, DatabaseType.SqlServer));
            }
        }

        /// <summary>
        /// 定义仓储（基础库）
        /// </summary>
        /// <returns></returns>
        public IRepository BaseRepository()
        {
            return new Repository(DbFactory.Base());
        }
        public IRepository DataRepository()
        {
            return new Repository(DbFactory.Data());
        }
    }
}
