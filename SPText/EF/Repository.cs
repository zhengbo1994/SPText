﻿using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace SPText.EF
{
    public class Repository<T> where T:class
    {
        private DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public Repository(DbContext _dbContext)
        {
            if (_dbContext==null)
            {
                throw new ArgumentNullException("错误！");
            }
            DbContext = _dbContext;
            DbSet = DbContext.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
                dbEntityEntry.State = EntityState.Added;
            else
                DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
                DbSet.Attach(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
                dbEntityEntry.State = EntityState.Deleted;
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Update(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Update(entity);
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);
        }


        public virtual void SqlQuery(string sql, params SqlParameter[] sqlParameter)
        {
            //string sql = "update Equipment set State='1' where Id=@Id";
            //SqlParameter[] pms = new SqlParameter[]{
            //    new SqlParameter("@Id",id)
            //};
            DbSet.SqlQuery(sql, sqlParameter);
        }
    }
}