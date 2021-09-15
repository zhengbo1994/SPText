using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPText.EF.EF2
{
    public interface IUnitWorkT<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// EF默认情况下，每调用一次SaveChanges()都会执行一个单独的事务
        /// 本接口实现在一个事务中可以多次执行SaveChanges()方法
        /// </summary>
        void ExecuteWithTransaction(Action action);
        /// <summary>
        /// 返回DbContext,用于多线程等极端情况
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
        /// <summary>
        /// 返回一个单独的实体，如果记录多于1个则取第一个
        /// </summary>
        T FirstOrDefault<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 判断指定条件的记录是否存在
        /// </summary>
        bool Any<T>(Expression<Func<T, bool>> exp) where T : class;
        IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class;

        IQueryable<T> Find<T>(int pageindex = 1, int pagesize = 10,
            Expression<Func<T, bool>> exp = null) where T : class;

        int Count<T>(Expression<Func<T, bool>> exp = null) where T : class;

        /// <summary>
        /// 新增对象，如果Id为空，则会自动创建默认Id
        /// </summary>
        void Add<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 批量新增对象，如果对象Id为空，则会自动创建默认Id
        /// </summary>
        void BatchAdd<T>(T[] entities) where T : BaseEntity;

        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;


        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update&lt;User&gt;(u =>u.Id==1,u =>new User{Name="ok"})</para>
        /// <para>该方法内部自动调用了SaveChanges()，需要ExecuteWithTransaction配合才能实现事务控制</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="entity">更新后的实体</param>
        void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class;
        /// <summary>
        /// 批量删除
        /// <para>该方法内部自动调用了SaveChanges()，需要ExecuteWithTransaction配合才能实现事务控制</para>
        /// </summary>
        void Delete<T>(Expression<Func<T, bool>> exp) where T : class;

        void Save();

        /// <summary>
        /// 执行Sql语句（增、删、改），返回受影响的行数
        /// 注意使用时，不要使用内插$参数
        /// 如有参数时，应该提供占位符："EXECUTE dbo.GetMostPopularBlogsForUser {0}", user
        /// 如需要内插，请使用ExecuteSqlInterpolated方法
        /// 该方法不支持EF自带的事务,需要ExecuteWithTransaction配合才能实现事务控制,详见：http://doc.openauth.me/core/unitwork.html
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 执行Sql语句（增、删、改），返回受影响的行数，
        /// 内插$参数时，会自动转换为 DbParameter，且不易受到 SQL 注入攻击。
        /// 如有参数时，直接使用内插："EXECUTE dbo.GetMostPopularBlogsForUser {user}"，
        /// 这里不需要对字符串值添加单引号包裹
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //int ExecuteSqlInterpolated(FormattableString sql);

        /// <summary>
        /// 使用SQL脚本查询
        /// </summary>
        /// <typeparam name="T"> T为数据库实体</typeparam>
        /// <returns></returns>
        //IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class;

        /// <summary>
        /// 使用SQL脚本查询
        /// </summary>
        /// <typeparam name="T"> T为非数据库实体，需要在DbContext中增加对应的DbQuery</typeparam>
        /// <returns></returns>
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class;

        #region 异步接口

        Task<int> ExecuteSqlCommandAsync(string sql);
        Task<int> SaveAsync();
        Task<int> CountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> exp) where T : class;
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> exp) where T : class;

        #endregion
    }


    public abstract class BaseEntity
    {

    }

    //
    // 摘要:
    //     Marks a type as keyless entity.
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class KeylessAttribute : Attribute
    {
        public KeylessAttribute()
        {

        }
    }
}
