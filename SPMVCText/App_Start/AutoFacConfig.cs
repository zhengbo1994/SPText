using Autofac;
using Autofac.Integration.Mvc;
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
    }
}