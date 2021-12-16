using Microsoft.EntityFrameworkCore;
using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Interface
{
    public partial class HkoMSContext : DbContext
    {
        public HkoMSContext(DbContextOptions<HkoMSContext> options) : base(options)
        {

        }

        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleElement> ModuleElements { get; set; }
        public virtual DbSet<Org> Orgs { get; set; }
        //public virtual DbSet<Relevance> Relevances { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        ////新增铺货功能的订单信息表
        //public virtual DbSet<PurchasingInformation> PurchasingInformation { get; set; }
        ////新增铺货功能的订单信息视图表
        //public virtual DbSet<PurchasingInformation_View> PurchasingInformation_View { get; set; }
        ////新增铺货功能的订单信息子表
        //public virtual DbSet<PurchasingInformationSub> PurchasingInformationSub { get; set; }
        ////新增铺货功能的权限状态表
        //public virtual DbSet<PurchasePermissionStatus> PurchasePermissionStatus { get; set; }
        ////新增铺货功能的审核人分类表
        //public virtual DbSet<PurchaseClassificationAuditors> PurchaseClassificationAuditors { get; set; }
        ////新增发送邮件给配置账户
        //public virtual DbSet<EmailSendAccountConfigure> EmailSendAccountConfigure { get; set; }
        ////新增铺货功能的邮件信息表
        //public virtual DbSet<EmailMessage> EmailMessage { get; set; }
    }
}
