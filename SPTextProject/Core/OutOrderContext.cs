using Microsoft.EntityFrameworkCore;
using SPTextProject.Common;
using SPTextProject.Models;
using SPTextProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.Core
{
    public partial class OutOrderContext : DbContext
    {
        public OutOrderContext(DbContextOptions<OutOrderContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (FactoryCache.CurrentFactory == null)
            {
                throw new Exception("当前工厂设置为空，不能执行任何工厂数据库操作");
            }
            optionsBuilder.EnableSensitiveDataLogging(true);  //允许打印参数
            string connStr = ConfigManager.GetConfig($"DbContext:Factories:{FactoryCache.CurrentFactory}:ConnStr");
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("当前工厂设置为空，不能执行任何工厂数据库操作");
            }
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<OutOrder> OutOrders { get; set; }

        public virtual DbSet<SequenceNumber> SequenceNumbers { get; set; }

        public virtual DbSet<OmaShape> OmaShapes { get; set; }
    }
}
