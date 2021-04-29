using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.EFBaseServices
{
    using LinqKit;
    using Microsoft.EntityFrameworkCore;
    using SPTextCommon.EFBaseServices.Model;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// 自定义EF上下文容器类
    /// </summary>
    public class BaseDBContext : System.Data.Entity.DbContext
    {
        public System.Data.Entity.DbContext Context { get; private set; }
        public BaseDBContext() : base("name=DataContext")
        {
            CreateBaseService("name=DataContext");
        }
        public BaseDBContext(string connection)
        {
            CreateBaseService(connection);
        }

        public void CreateBaseService(string connection)
        {
            this.Context = new System.Data.Entity.DbContext(connection);//连接数据库字符串
            this.Context.Configuration.ProxyCreationEnabled = false;//创建实体类型
            this.Context.Configuration.LazyLoadingEnabled = false;//延迟加载
            this.Context.Configuration.ValidateOnSaveEnabled = false;//上下文跟踪
            this.Context.Configuration.AutoDetectChangesEnabled = false;//上下文跟踪
            this.Context.Configuration.UseDatabaseNullSemantics = false;//上下文跟踪
            this.Context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;//上下文跟踪
        }
        public virtual System.Data.Entity.DbSet<Company> Company { get; set; }
    }

    /// <summary>
    /// 负责对当前数据库已经在.edmx中存在的相关表进行增，删，查，改
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDal<T> : IBaseDal<T> where T : class
    {
        //1.0 实例化上下文容器对象类
        //此处有一个缺点，会导致一个控制器类中如果有对个业务逻辑接口，则会存在多个EF容器对象
        //就会影响操作性能，也不方便编码
        //BaseDBContext db = new BaseDBContext();

        public BaseDBContext db
        {
            get
            {
                string threadCacheKey = typeof(BaseDBContext).FullName;
                //1.0 先问线程缓存是否有ef容器对象实例，如果返回为null则应该创建一个存入到线程缓存中
                object efInstance = CallContext.GetData(threadCacheKey);
                if (efInstance == null)
                {
                    //实例化ef容器对象
                    efInstance = new BaseDBContext();
                    //((IObjectContextAdapter)efInstance).ObjectContext.CommandTimeout = 180;//设置超时时间
                    //将efInstance存入线程缓存中
                    CallContext.SetData(threadCacheKey, efInstance);
                }

                //返回
                return efInstance as BaseDBContext;
            }
        }

        //2.0 定义泛型的DbSet<T> 字段
        protected System.Data.Entity.DbSet<T> _dbset;
        public BaseDal()
        {
            _dbset = db.Context.Set<T>();
        }

        #region 3.0 查询方法的封装

        #region 3.0.1 带条件查询

        public virtual List<T> QueryWhere(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ToList();
        }

        #endregion

        #region 3.0.2 连表带条件查询

        public virtual List<T> QueryJoin(Expression<Func<T, bool>> where, string[] tableNames)
        {
            ////1.0 参数合法性判断
            //if (tableNames.Any() == false)
            //{
            //    throw new Exception("tableNames至少要有一个数据");
            //}

            //DbQuery<T> query = _dbset;
            ////2.0 连表带条件查询
            //foreach (var tableName in tableNames)
            //{
            //    query = query.Include(tableName);
            //}

            //return query.Where(where).ToList();

            return null;
        }

        #endregion

        #region 3.0.3 带条件的分页查询
        public virtual List<T> QueryByPage<TKey>(int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, bool>> where
            , Expression<Func<T, TKey>> order)
        {
            //1.0 计算跳过的总行数
            int skipCount = (pageindex - 1) * pagesize;

            //2.0 获取满足条件的总行数
            totalcount = _dbset.Count(where);

            //3.0 查询
            return _dbset.Where(where).OrderByDescending(order).Skip(skipCount).Take(pagesize).ToList();
        }


        #endregion

        #region 3.0.4 统一调用SQL语句的方法 (用于查询)

        /// <summary>
        /// 如果使用此方法调用存储过程：
        /// sql: exec usp_getlist 1,2  -->EF中如何传入 usp_getlist 1,2 有sql注入的危险
        /// 参数化查询：usp_getlist @uid,@grouid ,同时将@uid,@grouid要通过pamrs传入
        /// 问题:只能获取一个存储过程的结果集，不能获取存储过程中的output类型的参数
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql">如果是调用有output参数的存储过程
        /// 则应该 Usp_GetAllContact9 @pageindex,@pagesize,@totalCount OUTPUT  
        /// 一定要加上 OUTPUT关键字，否则无值返回</param>
        /// <param name="pamrs"></param>
        /// <returns></returns>
        /// 

        /*
        调用存储过程
        string sql = "Usp_GetAllContact9 @pageindex,@pagesize,@totalCount OUTPUT ";
        System.Data.SqlClient.SqlParameter[] ps = new System.Data.SqlClient.SqlParameter[]{
         new System.Data.SqlClient.SqlParameter("@pageindex",1),
          new System.Data.SqlClient.SqlParameter("@pagesize",2),
           new System.Data.SqlClient.SqlParameter("@totalCount",0)
        };
        ps[2].Direction = ParameterDirection.Output;

        var list = gbll.RunSql<Usp_GetAllContact9_Result>(sql, ps).ToList();
        */
        public virtual List<TResult> RunSql<TResult>(string sql, params object[] pamrs)
        {
            return db.Database.SqlQuery<TResult>(sql, pamrs).ToList();
        }

        #endregion

        #region 3.0.5.1 负责多条件带分页查询的方法
        public List<T> QueryByMultiCondition<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, TKey>> order)
        {

            //1.0 计算跳过的总行数
            int skipCount = (pageindex - 1) * pagesize;

            //2.0 获取满足条件的总行数
            totalcount = _dbset.AsExpandable().Where(whereLambda).Count();
            //totalcount = _dbset.Count(whereLambda);
            return _dbset.AsExpandable().Where(whereLambda).AsQueryable().OrderByDescending(order).Skip(skipCount).Take(pagesize).ToList();

        }
        #endregion

        #region 3.0.5.2 负责多条件带分页查询的方法(正序)
        public List<T> QueryByMultiConditionOrderBy<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, TKey>> order)
        {

            //1.0 计算跳过的总行数
            int skipCount = (pageindex - 1) * pagesize;

            //2.0 获取满足条件的总行数
            totalcount = _dbset.AsExpandable().Where(whereLambda).Count();
            //totalcount = _dbset.Count(whereLambda);
            return _dbset.AsExpandable().Where(whereLambda).AsQueryable().OrderBy(order).Skip(skipCount).Take(pagesize).ToList();

        }
        #endregion

        #region 3.0.6 负责连表和多条件带分页查询的方法
        public virtual List<T> QueryByMultiConditionByJoin<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, TKey>> order, string[] tableNames)
        {


            ////1.0 计算跳过的总行数
            //int skipCount = (pageindex - 1) * pagesize;

            ////2.0 获取满足条件的总行数
            //totalcount = _dbset.AsExpandable().Where(whereLambda).Count();


            //DbQuery<T> query = _dbset;
            ////2.0 连表带条件查询
            //foreach (var tableName in tableNames)
            //{
            //    query = query.Include(tableName);
            //}


            ////totalcount = _dbset.Count(whereLambda);
            //return query.AsExpandable().Where(whereLambda).AsQueryable().OrderByDescending(order).Skip(skipCount).Take(pagesize).ToList();
            totalcount = 0;
            return null;
        }
        #endregion

        #region 3.0.7 负责多条件的普通查询的方法
        public virtual List<T> QueryByWhereLambda(Expression<Func<T, bool>> whereLambda)
        {

            return _dbset.AsExpandable().Where(whereLambda).AsQueryable().ToList();
        }
        #endregion

        #region 3.0.8 负责多条件的普通查询+外表連接的方法
        public virtual List<T> QueryByWhereJoin(Expression<Func<T, bool>> whereLambda, string[] tableNames)
        {
            //DbQuery<T> query = _dbset;
            ////2.0 连表带条件查询
            //foreach (var tableName in tableNames)
            //{
            //    query = query.Include(tableName);
            //}

            //return query.AsExpandable().Where(whereLambda).AsQueryable().ToList();

            return null;
        }
        #endregion

        #region 3.0.9 不跟踪查询
        public virtual List<T> QueryWhereAsNoTracking(Expression<Func<T, bool>> where)
        {
            return _dbset.AsNoTracking().Where(where).ToList();

        }
        #endregion


        #endregion

        #region 4.0 新增

        public virtual void Add(T model)
        {
            _dbset.Add(model);
        }

        #endregion

        #region 5.0 编辑

        /// <summary>
        /// 按需编辑，程序员指定需要更新的字段
        /// </summary>
        public virtual void Edit(T model, string[] propertys)
        {
            //1.0 将model追加到EF容器中
            var entry = db.Entry(model);
            entry.State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in propertys)
            {
                entry.Property(item).IsModified = true;
            }

            //2.0 关闭合法性验证
            db.Configuration.ValidateOnSaveEnabled = false;
        }


        public virtual void Edit(T model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
        }

        #endregion

        #region 6.0 物理删除
        public virtual void Delete(T model, bool isAdded)
        {
            //判断当前model如果没有追加到ef容器，则使用Attach进行追加
            if (!isAdded)
            {
                _dbset.Attach(model);
            }

            //将当前代理类的状态修改成Deleted
            _dbset.Remove(model);
        }



        #endregion

        #region 7.0 统一执行SQL语句

        public virtual int SaveChanges()
        {
            return db.SaveChanges();
        }

        #endregion

        #region 8.0 调用只有一个返回值的存储过程
        public virtual ObjectResult<Tkey> RunProp<Tkey>(string propName, ObjectParameter parm)
        {
            return ((IObjectContextAdapter)db).ObjectContext.ExecuteFunction<Tkey>(propName, parm);


        }
        #endregion
    }
}
