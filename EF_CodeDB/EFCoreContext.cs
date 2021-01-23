//using Asp.NetCore.EFCore.Models.Extend;
//using Asp.NetCore.EFCore.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EF_CodeDB
{
    public partial class EFCoreContext : DbContext
    {

        ///记录控制台日志：
        ///1.  Microsoft.Extensions.Logging
        ///Microsoft.Extensions.Logging.Console 
        ///2. 定义日志工厂
        ///3. OnConfiguring配置使用日志工厂 
        /// <summary>
        /// 指定静态ILoggerFactory
        /// </summary>
        public static readonly ILoggerFactory MyLoggerFactory= LoggerFactory.Create(builder => { builder.AddConsole(); });

        public EFCoreContext()
        {

        }

        public EFCoreContext(DbContextOptions<EFCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Conmpany { get; set; }
        public virtual DbSet<SysLog> SysLog { get; set; }
        public virtual DbSet<SysLogDetail> SysLogDetail { get; set; }
        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysUserInfoDetail> SysUserInfoDetail { get; set; }
        public virtual DbSet<SysUserRoleMapping> SysUserRoleMapping { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }


        //public EFCoreContext(IOptions<>)
        //{ 
        
        //}



        private string Conn = null;
         
        public DbContext ToWriteOrRead(string conn)
        {
            Conn = conn;
            return this;
        }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLoggerFactory(MyLoggerFactory)
                     .UseLazyLoadingProxies()
                    .UseSqlServer(Conn);
            }
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasKey(a => a.Id);
            //modelBuilder.Entity<SysUser>().HasOne(u => u.Company).WithMany().HasForeignKey(u => u.CompanyId);


            #region MyRegion
            /////设置数据库架构
            //modelBuilder.HasDefaultSchema("Zhaoxi");

            /////表明、属性名映射
            //modelBuilder.Entity<UserInfo>().ToTable("UserInfos", "Zhaoxi").Property(p => p.UserAge).HasColumnName("Age");

            ////初始化数据
            //modelBuilder.Entity<Company>().HasData(new List<Company>()
            //{

            //});

            //modelBuilder.Entity<Company>().HasData()

            // ////设置一对一的关系；
            //modelBuilder.Entity<SysLog>().HasOne(u => u.SysLogDetail).WithOne().HasForeignKey<SysUser>(s => s.Id);

            //////设置一对多的关系
            ////modelBuilder.Entity<SysUser>().HasOne(c => c.Company).WithMany(s => s.SysUser).HasForeignKey(b => b.CompanyId);

            ////多对多关系  
            modelBuilder.Entity<SysUserRoleMapping>().HasOne(p => p.SysUser)
                .WithMany(u => u.SysUserRoleMapping).HasForeignKey(u => u.SysUserId);

            //modelBuilder.Entity<SysUserRoleMapping>().HasOne(p => p.SysRole)
            //    .WithMany(r => r.SysUserRoleMapping).HasForeignKey(s => s.SysRoleId);

            modelBuilder.Entity<SysUserRoleMapping>().HasKey(p => new { p.SysUserId, p.SysRoleId }); //设置联合主键

            /////表拆分:在数据库中是一整张表，在代码层面是多个实体与其对应；
            modelBuilder.Entity<SysLog>(dob =>
            {
                dob.ToTable("SysLogInfo");
                dob.Property(o => o.LogType).HasColumnName("LogType");//配置两个实体的相同属性映射到表的同一列
                dob.HasOne(o => o.SysLogDetail).WithOne().HasForeignKey<SysLog>(o => o.Id); ; //配置两个实体的相同属性映射到表的同一列
            });

            modelBuilder.Entity<SysLogDetail>(dob =>
            {
                dob.ToTable("SysLogInfo");
                dob.Property(o => o.LogType).HasColumnName("LogType");//配置两个实体的相同属性映射到表的同一列 
            });
            #endregion


            modelBuilder.Entity<SysLogDetail>(ob =>
            {
                //    ob.ToTable("SysLogInfo");
                //    ob.Property(o => o.LogType).HasColumnName("LogType");
                //}); //配置关系表共享，一个表拆分成两个实体

                //modelBuilder.Entity<Company>(entity =>
                //{
                //    entity.Property(e => e.CompanyName).HasMaxLength(50);
            });


            modelBuilder.Entity<SysLog>(entity =>
            {
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<SysMenu>(entity =>
            {

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.MenuIcon).HasMaxLength(20);

                entity.Property(e => e.SourcePath).HasMaxLength(1000);

                //entity.Property(e => e.SysMenuName)
                //    .IsRequired()
                //    .HasMaxLength(100);

                entity.Property(e => e.Url).HasMaxLength(500);
            });


            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<SysUser>(entity =>
            {

                //entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => new { e.Name, e.Phone });

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Phone).HasMaxLength(20);

                //entity.Property(e => e.Qq).HasColumnName("QQ");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.WeChat).HasMaxLength(50);

            });

            modelBuilder.Entity<SysUserInfoDetail>(entity =>
            {
                entity.HasIndex(e => e.SysUserInfoDetailId)
                    .IsUnique();

                entity.HasIndex(e => e.SysUserInfoId);
            });

            modelBuilder.Entity<SysUserRoleMapping>(entity =>
            {
                entity.HasKey(e => new { e.SysUserId, e.SysRoleId });

                entity.HasIndex(e => e.SysRoleId);
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //Context SaveChange 是所有操作一次提交，ChangeTracker.Entries()包含了所有更改的内容； 
        public override int SaveChanges()
        {
           //穿插一些自己动作
            {
                //ChangeTracker.Entries().Where(s => s is AbstractBaseModel && s.State == EntityState.Modified).ToList().ForEach(e => ((AbstractBaseModel)e.Entity).LastModifyTime = DateTime.Now);
            }
            return base.SaveChanges();
        }
    }

     
}
