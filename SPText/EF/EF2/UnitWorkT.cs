﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace SPText.EF.EF2
{
    public class UnitWorkT<TDbContext> : IUnitWorkT<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _context;

        public UnitWorkT(TDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// EF默认情况下，每调用一次SaveChanges()都会执行一个单独的事务
        /// 本接口实现在一个事务中可以多次执行SaveChanges()方法
        /// </summary>
        public void ExecuteWithTransaction(Action action)
        {
            using (DbContextTransaction transaction = _context.Database.BeginTransaction())//事务
            {
                try
                {
                    action();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 返回DbContext,用于多线程等极端情况
        /// </summary>
        public DbContext GetDbContext()
        {
            return _context;
        }

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp">The exp.</param>
        public IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp);
        }

        public bool Any<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().Any(exp);
        }

        /// <summary>
        /// 查找单个
        /// </summary>
        public T FirstOrDefault<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="orderby">排序，格式如："Id"/"Id descending"</param>
        public IQueryable<T> Find<T>(int pageindex, int pagesize, Expression<Func<T, bool>> exp = null) where T : class
        {
            if (pageindex < 1) pageindex = 1;

            return Filter(exp).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        public int Count<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp).Count();
        }

        /// <summary>
        /// 新增对象，如果Id为空，则会自动创建默认Id
        /// </summary>
        public void Add<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Add(entity);
        }

        /// <summary>
        /// 批量新增对象，如果对象Id为空，则会自动创建默认Id
        /// </summary>
        public void BatchAdd<T>(T[] entities) where T : BaseEntity
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Update<T>(T entity) where T : class
        {
            var entry = this._context.Entry(entity);
            entry.State = EntityState.Modified;

            //如果数据没有发生变化
            if (!this._context.ChangeTracker.HasChanges())
            {
                entry.State = EntityState.Unchanged;
            }

        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update&lt;User&gt;(u =>u.Id==1,u =>new User{Name="ok"})</para>
        /// <para>该方法内部自动调用了SaveChanges()，需要ExecuteWithTransaction配合才能实现事务控制</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="entity">更新后的实体</param>
        public void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class
        {
            _context.Set<T>().Where(where).Update(entity);
        }

        /// <summary>
        /// 批量删除
        /// <para>该方法内部自动调用了SaveChanges()，需要ExecuteWithTransaction配合才能实现事务控制</para>
        /// </summary>
        public virtual void Delete<T>(Expression<Func<T, bool>> exp) where T : class
        {
            _context.Set<T>().Where(exp).Delete();
        }

        public void Save()
        {
            try
            {
                var entities = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added
                                || e.State == EntityState.Modified)
                    .Select(e => e.Entity);

                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }

                _context.SaveChanges();
            }
            catch (ValidationException exc)
            {
                Console.WriteLine($"{nameof(Save)} validation exception: {exc?.Message}");
                throw (exc.InnerException as Exception ?? exc);
            }
            catch (Exception ex) //DbUpdateException 
            {
                throw (ex.InnerException as Exception ?? ex);
            }

        }

        private IQueryable<T> Filter<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
            //var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            //if (exp != null)
            //    dbSet = dbSet.Where(exp);
            //return dbSet;
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        //public int ExecuteSqlInterpolated(FormattableString sql)
        //{
        //    return _context.Database.ExecuteSqlInterpolated(sql);
        //}

        //public IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class
        //{
        //    return _context.Set<T>().FromSqlRaw(sql, parameters);
        //}

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
        {
            return _context.Database.SqlQuery<T>(sql, parameters);
        }

        #region 异步实现

        /// <summary>
        /// 异步执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlCommandAsync(string sql)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sql);
        }


        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> SaveAsync()
        {
            try
            {
                var entities = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added
                                || e.State == EntityState.Modified)
                    .Select(e => e.Entity);

                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }

                return await _context.SaveChangesAsync();
            }
            catch (ValidationException exc)
            {
                Console.WriteLine($"{nameof(Save)} validation exception: {exc?.Message}");
                throw (exc.InnerException as Exception ?? exc);
            }
            catch (Exception ex) //DbUpdateException 
            {
                throw (ex.InnerException as Exception ?? ex);
            }
        }

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return await Filter(exp).CountAsync();
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return await _context.Set<T>().AnyAsync(exp);
        }

        /// <summary>
        /// 查找单个，且不被上下文所跟踪
        /// </summary>
        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(exp);
        }

        #endregion
    }

    #region  继承DbContext
    public partial class InOrderContext : DbContext
    {
        #region Identity
        protected DbContext Context { get; private set; }
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="context"></param>
        public InOrderContext(DbContext context)
        {
            this.Context = context;
        }

        public InOrderContext() : base("name=ConnStr")
        {
            CreateBaseService("name=ConnStr");
        }

        public InOrderContext(string connection) : base(connection)
        {
            CreateBaseService(connection);
        }

        public void CreateBaseService(string connection)
        {
            this.Context = new DbContext(connection);//连接数据库字符串

        }
        #endregion Identity

        public virtual DbSet<User> User { get; set; }

    }
    public class User:BaseEntity {
        public int Id { get; set; }
    }
    #endregion
}
