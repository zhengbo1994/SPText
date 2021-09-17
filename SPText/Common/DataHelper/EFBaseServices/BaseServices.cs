using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.EFBaseServices
{
    public class BaseServices<T> : IBaseServices<T> where T : class
    {
        //1.0 实例化BaseDal对象

        /// <summary>
        /// 此成员会被继承于BaseServices 的子类构造函数初始化
        /// </summary>
        protected IBaseDal<T> basedal;

        #region 3.0 查询方法的封装

        #region 3.0.1 带条件查询

        public virtual List<T> QueryWhere(Expression<Func<T, bool>> where)
        {
            return basedal.QueryWhere(where);
        }

        #endregion

        #region 3.0.2 连表带条件查询

        public virtual List<T> QueryJoin(Expression<Func<T, bool>> where, string[] tableNames)
        {
            return basedal.QueryJoin(where, tableNames);
        }

        #endregion

        #region 3.0.3 带条件的分页查询
        public virtual List<T> QueryByPage<TKey>(int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, bool>> where
            , Expression<Func<T, TKey>> order)
        {
            return basedal.QueryByPage<TKey>(pageindex, pagesize, out totalcount, where, order);
        }


        #endregion

        #region 3.0.4 统一调用SQL语句的方法 (用于查询)

        /// <summary>
        /// 如果使用此方法调用存储过程：
        /// sql: exec usp_getlist 1,2  -->EF中如何传入 usp_getlist 1,2 有sql注入的危险
        /// 参数化查询：usp_getlist @uid,@grouid ,同时将@uid,@grouid要通过pamrs传入
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pamrs"></param>
        /// <returns></returns>
        public virtual List<TResult> RunSql<TResult>(string sql, params object[] pamrs)
        {
            return basedal.RunSql<TResult>(sql, pamrs);
        }

        #endregion

        #region 3.0.5.1 负责多条件查询的方法（倒序）
        public virtual List<T> QueryByMultiCondition<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
             , int pagesize
             , out int totalcount
             , Expression<Func<T, TKey>> order)
        {

            return basedal.QueryByMultiCondition(whereLambda, pageindex, pagesize, out totalcount, order).Select(p => p).ToList();
        }

        #endregion

        #region 3.0.5.2 负责多条件查询的方法(正序)
        public virtual List<T> QueryByMultiConditionOrderBy<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
             , int pagesize
             , out int totalcount
             , Expression<Func<T, TKey>> order)
        {

            return basedal.QueryByMultiConditionOrderBy(whereLambda, pageindex, pagesize, out totalcount, order).Select(p => p).ToList();
        }

        #endregion

        #region 3.0.6 负责连表和多条件带分页查询的方法
        public virtual List<T> QueryByMultiConditionByJoin<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
           , int pagesize
           , out int totalcount
           , Expression<Func<T, TKey>> order, string[] tableNames)
        {

            return basedal.QueryByMultiConditionByJoin(whereLambda, pageindex, pagesize, out totalcount, order, tableNames).Select(p => p).ToList();
        }
        #endregion

        #region 3.0.7 负责多条件的普通查询的方法
        public virtual List<T> QueryByWhereLambda(Expression<Func<T, bool>> whereLambda)
        {

            return basedal.QueryByWhereLambda(whereLambda).ToList();
        }
        #endregion

        #region 3.0.8 负责多条件的普通查询+外表連接的方法
        public virtual List<T> QueryByWhereJoin(Expression<Func<T, bool>> whereLambda, string[] tableNames)
        {

            return basedal.QueryByWhereJoin(whereLambda, tableNames).ToList();
        }
        #endregion

        #region 3.0.9 不跟踪查询
        /// <summary>
        /// 不跟踪查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual List<T> QueryWhereAsNoTracking(Expression<Func<T, bool>> where)
        {
            return basedal.QueryWhereAsNoTracking(where);
        }
        #endregion
        #endregion

        #region 4.0 新增

        public virtual void Add(T model)
        {
            basedal.Add(model);
        }

        #endregion

        #region 5.0 编辑

        /// <summary>
        /// 按需编辑，程序员指定需要更新的字段
        /// </summary>
        public virtual void Edit(T model, string[] propertys)
        {
            basedal.Edit(model, propertys);
        }

        public virtual void Edit(T model)
        {
            basedal.Edit(model);
        }

        #endregion

        #region 6.0 物理删除
        public virtual void Delete(T model, bool isAdded)
        {
            basedal.Delete(model, isAdded);
        }



        #endregion

        #region 7.0 统一执行SQL语句

        public virtual int SaveChanges()
        {
            return basedal.SaveChanges();
        }

        #endregion

        #region 8.0 调用只有一个返回值的存储过程
        public virtual ObjectResult<Tkey> RunProp<Tkey>(string propName, ObjectParameter parm)
        {
            return basedal.RunProp<Tkey>(propName, parm);
        }
        #endregion
    }
}
