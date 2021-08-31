using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SPTextProject.Common;
using SPTextProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SPTextProject.Core
{
    public partial class InOrderContext : DbContext
    {
        public InOrderContext(DbContextOptions<InOrderContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);  //允许打印参数
            string connStr = ConfigManager.GetConfig("DbContext:OrderCenter:ConnStr");
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<InOrder> InOrders { get; set; }

        public virtual DbSet<OriginalOrderInfo> OriginalOrderInfos { get; set; }
    }
}
