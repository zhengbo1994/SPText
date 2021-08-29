using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using SPMVCText.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace SPMVCText.App_Start
{
    public class AutoFacConfig
    {
        public static void Register()
        {
            //TODO:使用autofanc实现的工厂替换MVC底层默认控制器工厂
            //TODO:还用autofac将所有的接口初始化

            //1.0 实例化autofac的容器创建者的对象
            var bulider = new ContainerBuilder();

            //2.0 实例化仓储层的所有接口的实现类的对象，以接口和实现类的对象形式存储在autofac容器内存中
            //bulider.RegisterType(typeof(sysFunctionRepository)).As(typeof(IsysFunctionRepository));
            //bulider.RegisterType(typeof(sysKeyValueRepository)).As(typeof(IsysKeyValueRepository));
            bulider.RegisterTypes(Assembly.Load("MD.MES.Repository").GetTypes()).AsImplementedInterfaces();
            //3.0 实例化Services（业务逻辑层的接口）
            //bulider.RegisterType(typeof(sysFunctionServices)).As(typeof(IsysFunctionServices));
            //bulider.RegisterType(typeof(sysKeyValueServices)).As(typeof(IsysKeyValueServices));
            bulider.RegisterTypes(Assembly.Load("MD.MES.Services").GetTypes()).AsImplementedInterfaces();

            //4.0 告诉autofac将来创建控制器类对象的程序集名称为什么
            Assembly ass = Assembly.Load("MD.MES.Site");
            bulider.RegisterControllers(ass);

            //5.0 告诉auto发出容器创建者创建一个auto的正真容器对象
            var container = bulider.Build();

            //6.0 将当前的autofac容器对象存入全局缓存中
            CacheMgr.SetData("mesautofaccontainer", container);

            //6.0 告诉MVC将DefaultControllerFactory替换成autofac中的控制器创建工厂
            //将来所有的接口使用container去进行传递
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public IContainer Register1()
        {
            var builder = new ContainerBuilder();
            AddApplicationDI(builder);


            builder.RegisterType<AutoMapperProfiles>().As<Profile>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            IContainer Container = builder.Build(Autofac.Builder.ContainerBuildOptions.None);
            return Container;
        }

        public void AddApplicationDI(ContainerBuilder builder)
        {
            //builder.RegisterType<UserService>().As<IUserService>();
            //builder.RegisterType<SubUserService>().As<ISubUserService>();
            //builder.RegisterType<SubDbUserService>().As<ISubDbUserService>();

            AddDomainDI(builder);
        }

        public void AddDomainDI(ContainerBuilder builder)
        {
            //builder.RegisterType<UserRepository>().As<IUserRepository>();
            //builder.RegisterType<SubUserRepository>().As<ISubUserRepository>();
            //builder.RegisterType<SubDbUserRepository>().As<ISubDbUserRepository>();
        }

        public static void Register2()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetCallingAssembly())//注册mvc的Controller
                .PropertiesAutowired();//属性注入


            //1、无接口类注入
            //builder.RegisterType<BLL.newsClassBLL>().AsSelf().InstancePerRequest().PropertiesAutowired();


            ////2、有接口类注入
            ////注入BLL，UI中使用
            //builder.RegisterAssemblyTypes(typeof(BLL.BaseBLL<>).Assembly)
            //    .AsImplementedInterfaces()  //是以接口方式进行注入
            //    .InstancePerRequest()       //每次http请求
            //    .PropertiesAutowired();     //属性注入

            ////注入DAL，BLL层中使用
            //builder.RegisterAssemblyTypes(typeof(DAL.BaseDAL<>).Assembly).AsImplementedInterfaces()
            //    .InstancePerRequest().PropertiesAutowired();     //属性注入

            //Cache的注入，使用单例模式
            //builder.RegisterType<RedisCacheManager>()
            //    .As<ICacheManager>()
            //    .SingleInstance()
            //    .PropertiesAutowired();

            //移除原本的mvc的容器，使用AutoFac的容器，将MVC的控制器对象实例交由autofac来创建
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Input
            //CreateMap<UserPostModel, UserEntity>();
            //CreateMap<UserPostModel, SubUserEntity>();
            //CreateMap<UserPostModel, SubDbUserEntity>();
            #endregion

            #region Output
            //CreateMap<UserEntity, UserModel>();
            //CreateMap<SubUserEntity, UserModel>();
            //CreateMap<SubDbUserEntity, UserModel>();
            #endregion
        }
    }
}