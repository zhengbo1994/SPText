using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SPCoreText.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SPCoreText.Common
{
    public static class AutofacExt
    {
        private static IContainer _container;

        /// <summary>
        /// 为控制反转容器提供一个控制器
        /// </summary>
        /// <param name="services"></param>
        /// <returns>控制器</returns>
        public static IContainer InitAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            //注册数据库基础操作和工作单元
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));//注册数据库基础操作//当需要一个继承于IRepository接口的类的对象时，使用BaseRepository（均为泛型）
            services.AddScoped(typeof(IUnitWork), typeof(UnitWork));//注册工作单元

            services.AddScoped(typeof(IAuth), typeof(LocalAuth));//注册权限管理



            ////通用接口 根本不能用啊啊啊啊
            //services.AddScoped(typeof(IRepositoryBase<>), typeof(BaseRepositoryBase<>));
            //services.AddScoped(typeof(IUnitWorkBase), typeof(UnitWorkBase));

            //如果想使用WebApi SSO授权，请使用下面这种方式
            //services.AddScoped(typeof(IAuth), typeof(ApiAuth));

            //注册app层
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

     

            //防止单元测试时已经注入
            //注册缓存上下文
            if (services.All(u => u.ServiceType != typeof(ICacheContext)))
            {
                services.AddScoped(typeof(ICacheContext), typeof(CacheContext));
            }

            //注册存取器
            if (services.All(u => u.ServiceType != typeof(IHttpContextAccessor)))
            {
                services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            }

            //隶属于
            builder.Populate(services);

            _container = builder.Build();
            return _container;

        }

        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
