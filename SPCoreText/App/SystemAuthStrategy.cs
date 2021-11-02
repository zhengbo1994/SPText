﻿using SPCoreText.Interface;
using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.BulkOperations.Internal.Data.SqlClient;

namespace SPCoreText.App
{
    /// <summary>
    /// 领域服务
    /// <para>超级管理员权限</para>
    /// <para>超级管理员使用guid.empty为ID，可以根据需要修改</para>
    /// </summary>
    public class SystemAuthStrategy : BaseApp<User>, IAuthStrategy
    {
        protected User _user;
        private DbExtension _dbExtension;

        /// <summary>
        /// 获取所有Modules及其所对应的Elements
        /// </summary>
        public List<ModuleView> Modules
        {
            get
            {
                var modules = (from module in UnitWork.Find<Module>(null)
                               select new ModuleView
                               {
                                   Name = module.Name,
                                   Id = module.Id,
                                   CascadeId = module.CascadeId,
                                   Code = module.Code,
                                   IconName = module.IconName,
                                   Url = module.Url,
                                   ParentId = module.ParentId,
                                   ParentName = module.ParentName,
                                   IsSys = module.IsSys
                               }).ToList();

                foreach (var module in modules)
                {
                    module.Elements = UnitWork.Find<ModuleElement>(u => u.ModuleId == module.Id).ToList();
                }

                return modules;
            }
        }

        public List<Role> Roles
        {
            get { return UnitWork.Find<Role>(null).ToList(); }
        }

        public List<ModuleElement> ModuleElements
        {
            get { return UnitWork.Find<ModuleElement>(null).ToList(); }
        }

        //public List<Resource> Resources
        //{
        //    get { return UnitWork.Find<Resource>(null).ToList(); }
        //}

        public List<Org> Orgs
        {
            get { return UnitWork.Find<Org>(null).ToList(); }
        }

        public User User
        {
            get { return _user; }
            set   //禁止外部设置
            {
                throw new Exception("超级管理员，禁止设置用户");
            }
        }

        public List<KeyDescription> GetProperties(string moduleCode)
        {
            return _dbExtension.GetProperties(moduleCode);
        }


        public SystemAuthStrategy(IUnitWork unitWork, IRepository<User> repository, DbExtension dbExtension) : base(unitWork, repository)
        {
            _dbExtension = dbExtension;
            _user = new User
            {
                Account = "System",
                Name = "超级管理员",
                Id = Guid.Empty.ToString()
            };
        }
    }
}