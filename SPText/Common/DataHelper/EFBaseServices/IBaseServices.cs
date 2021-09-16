using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.EFBaseServices
{
    public interface IBaseServices<T> where T : class
    {
        #region 3.0 查询方法的封装

        #region 3.0.1 带条件查询

        List<T> QueryWhere(Expression<Func<T, bool>> where);


        #endregion

        #region 3.0.2 连表带条件查询

        List<T> QueryJoin(Expression<Func<T, bool>> where, string[] tableNames);


        #endregion

        #region 3.0.3 带条件的分页查询
        List<T> QueryByPage<TKey>(int pageindex
           , int pagesize
           , out int totalcount
           , Expression<Func<T, bool>> where
           , Expression<Func<T, TKey>> order);



        #endregion

        #region 3.0.4 统一调用SQL语句的方法 (用于查询)

        /// <summary>
        /// 如果使用此方法调用存储过程：
        /// sql: exec usp_getlist 1,2  -->EF中如何传入 usp_getlist 1,2 有sql注入的危险
        /// 参数化查询：usp_getlist @uid,@grouid ,同时将@uid,@grouid要通过pamrs传入
        /// 问题:只能获取一个存储过程的结果集，不能获取存储过程中的output类型的参数
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pamrs"></param>
        /// <returns></returns>
        List<TResult> RunSql<TResult>(string sql, params object[] pamrs);


        #endregion

        #region 3.0.5.1 负责多条件查询的方法(倒序)
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<T> QueryByMultiCondition<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, TKey>> order);

        #endregion

        #region 3.0.5 负责多条件查询的方法（正序）
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<T> QueryByMultiConditionOrderBy<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
            , int pagesize
            , out int totalcount
            , Expression<Func<T, TKey>> order);

        #endregion

        #region 3.0.6 负责连表和多条件带分页查询的方法
        /// <summary>
        /// 负责连表和多条件带分页查询的方法
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalcount"></param>
        /// <param name="order"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        List<T> QueryByMultiConditionByJoin<TKey>(Expression<Func<T, bool>> whereLambda, int pageindex
           , int pagesize
           , out int totalcount
           , Expression<Func<T, TKey>> order, string[] tableNames);

        #endregion

        #region 3.0.7 负责多条件的普通查询的方法
        /// <summary>
        /// 负责多条件的普通查询的方法
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<T> QueryByWhereLambda(Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 3.0.8 负责多条件的普通查询+外表連接的方法
        /// <summary>
        /// 负责多条件的普通查询+外表連接的方法
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        List<T> QueryByWhereJoin(Expression<Func<T, bool>> whereLambda, string[] tableNames);
        #endregion

        #region 3.0.9 不跟踪查询
        List<T> QueryWhereAsNoTracking(Expression<Func<T, bool>> where);
        #endregion

        #endregion

        #region 4.0 新增

        void Add(T model);


        #endregion

        #region 5.0 编辑

        /// <summary>
        /// 按需编辑，程序员指定需要更新的字段
        /// </summary>
        void Edit(T model, string[] propertys);

        void Edit(T model);
        #endregion

        #region 6.0 物理删除
        void Delete(T model, bool isAdded);

        #endregion

        #region 7.0 统一执行SQL语句

        int SaveChanges();


        #endregion

        #region 8.0 调用只有一个返回值的存储过程
        ObjectResult<Tkey> RunProp<Tkey>(string propName, ObjectParameter parm);

        #endregion

    }
}
