using Microsoft.Practices.Unity.Configuration;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Resolution;

namespace SPText.Common.UnityIoc
{
    public class UnityIocHelper : IServiceProvider
    {
        private readonly IUnityContainer _container;

        private static readonly UnityIocHelper dbinstance = new UnityIocHelper("DBcontainer");

        private UnityIocHelper(string containerName)
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            _container = new UnityContainer();
            section.Configure(_container, containerName);
            //section.Configure(_container);  
        }

        //private static Readonly UnityIocHelper dbinstance = new UnityIocHelper("DBcontainer");
        //private UnityIocHelper(string containername)
        //{
        //    UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
        //    _container = new UnityContainer();
        //    section.Configure(_container, containername);
        //}


        public static string GetmapToByName(string containerName, string itype, string name = "")
        {
            try
            {
                UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                var _Containers = section.Containers;
                foreach (var _Container in _Containers)
                {
                    if (_Container.Name == containerName)
                    {
                        var _Registrations = _Container.Registrations;
                        foreach (var _Registration in _Registrations)
                        {
                            if (name == "" && string.IsNullOrEmpty(_Registration.Name) && _Registration.TypeName == itype)
                            {
                                return _Registration.MapToName;
                            }
                        }
                        break;
                    }
                }
                return "";
            }
            catch
            {
                throw;
            }

        }


        public static UnityIocHelper DBInstance
        {
            get { return dbinstance; }
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
        public T GetService<T>()
        {
            return _container.Resolve<T>();
        }
        public T GetService<T>(params ParameterOverride[] obj)
        {
            return _container.Resolve<T>(obj);
        }
        public T GetService<T>(string name, params ParameterOverride[] obj)
        {
            return _container.Resolve<T>(name, obj);
        }

        ///// <summary>  
        ///// 连接数据库  
        ///// </summary>  
        ///// <param name="connString">连接字符串</param>  
        ///// <param name="DbType">数据库类型</param>  
        ///// <returns></returns>  
        //public static IDatabase Base(string connString, DatabaseType DbType)
        //{
        //    DbHelper.DbType = DbType;
        //    return UnityIocHelper.DBInstance.GetService<IDatabase>(new ParameterOverride(
        //      "connString", connString), new ParameterOverride(
        //      "DbType", DbType.ToString()));
        //}
    }

    public class CustomUnityIocHelper
    {
        public void Show()
        {
            ////实例化容器对象
            //IHTContainer container = new HTContainer();
            ////注册对象
            //container.RegisterType<IDatabase, SqlserverDal>();
            ////通过容器完成对象的创建，不体现细节，用抽象完成对象的创建
            //IDatabase dal = container.Resolve<IDatabase>();
            //dal.Connection("con");

        }


        public class HTContainer : IHTContainer
        {
            //创建一个Dictionary数据类型的对象用来存储注册的对象
            private Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type>();

            //注册方法，用接口的FullName为key值，value为要创建对象的类型
            public void RegisterType<IT, T>()
            {
                this.TypeDictionary.Add(typeof(IT).FullName, typeof(T));
            }

            //创建对象通过传递的类型进行匹配
            public IT Resolve<IT>()
            {
                return (IT)this.ResolveObject(typeof(IT));
            }

            //通过递归的方式创建多层级的对象
            private object ResolveObject(Type abstractType)
            {
                //获取要创建对象的类型
                string key = abstractType.FullName;
                Type type = this.TypeDictionary[key];

                //获取对象的所有构造函数
                var ctorArray = type.GetConstructors();

                ConstructorInfo ctor = null;
                //判断构造函数中是否标记了HTAttribute这个特性
                if (ctorArray.Count(c => c.IsDefined(typeof(HTAttribute), true)) > 0)
                {
                    //若标记了HTAttribute特性，默认就采用这个构造函数
                    ctor = ctorArray.FirstOrDefault(c => c.IsDefined(typeof(HTAttribute), true));
                }
                else
                {
                    //若都没有标记特性，那就采用构造函数中参数最多的构造函数
                    ctor = ctorArray.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
                }

                //多个参数的形式
                List<object> paraList = new List<object>();

                foreach (var para in ctor.GetParameters())
                {
                    Type paraInterfaceType = para.ParameterType;
                    Type paraType = this.TypeDictionary[paraInterfaceType.FullName];
                    object oPara = ResolveObject(paraInterfaceType);  //自已调用自己，实现递归操作，完成各个层级对象的创建
                    paraList.Add(oPara);
                }

                return (object)Activator.CreateInstance(type, paraList.ToArray());
            }
        }
        public class HTAttribute : Attribute
        {
        }

        public interface IHTContainer
        {
            void RegisterType<IT, T>();
            IT Resolve<IT>();
        }
    }


    #region 其它



    public class IocHelper : IIocHelper, IDisposable
    {
        public void Show()
        {
            //IIocHelper ioc = IocFactory.Instance;
            //var actualContainer = ioc.Container as SimpleInjector.Container;
            //actualContainer.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            //var defaultLifeStyle = IocLifeStyle.Transient;

            //ioc.RegisterIoc<ICouponService, CouponService>(defaultLifeStyle);

            //actualContainer.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            //actualContainer.RegisterMvcIntegratedFilterProvider();
            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(actualContainer));
        }



        private SimpleInjector.Container _container;
        protected Dictionary<Type, IocRegistration> _typeRegistrationInfo = new Dictionary<Type, IocRegistration>();

        public IocHelper()
        {
            _container = new SimpleInjector.Container();
        }

        public object Container
        {
            get
            {
                return _container;
            }
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }

        public TInterface GetInstance<TInterface>()
            where TInterface : class
        {
            return _container.GetInstance<TInterface>();
        }

        public IocRegistration GetRegistration(Type serviceType)
        {
            if (_typeRegistrationInfo.ContainsKey(serviceType))
            {
                return _typeRegistrationInfo[serviceType];
            }
            return null;
        }

        protected Lifestyle GetLifestyle(IocLifeStyle iocLifestyle)
        {
            Lifestyle lifeStyle = Lifestyle.Scoped;
            switch (iocLifestyle)
            {
                case IocLifeStyle.Scoped:
                    lifeStyle = Lifestyle.Scoped;
                    break;
                case IocLifeStyle.Singleton:
                    lifeStyle = Lifestyle.Singleton;
                    break;
                case IocLifeStyle.Transient:
                    lifeStyle = Lifestyle.Transient;
                    break;
                default:
                    throw new NotImplementedException(String.Format("Unknown lifestyle:{0}", Enum.GetName(typeof(IocLifeStyle), iocLifestyle)));
            }
            return lifeStyle;
        }

        public void RegisterIoc(Type serviceType, Type implementationType, IocLifeStyle iocLifestyle)

        {
            Lifestyle lifeStyle = GetLifestyle(iocLifestyle);
            var prevSettings = GetRegistration(serviceType);
            if (prevSettings == null || prevSettings.ImplementationType != implementationType)
            {
                _container.Register(serviceType, implementationType, lifeStyle);
                _typeRegistrationInfo[serviceType] = new IocRegistration
                {
                    Container = _container,
                    ImplementationType = implementationType,
                    LifeStyle = iocLifestyle
                };
            }
        }

        public void RegisterIoc<TService>(Func<TService> funcImpl, IocLifeStyle iocLifestyle) where TService : class
        {
            Lifestyle lifeStyle = GetLifestyle(iocLifestyle);
            var reg = GetRegistration(typeof(TService));
            if (reg != null) return;
            _container.Register<TService>(funcImpl, lifeStyle);
            _typeRegistrationInfo[typeof(TService)] = new IocRegistration
            {
                Container = _container,
                LifeStyle = iocLifestyle,
                ObjectCreator = funcImpl
            };
        }

        public void RegisterIoc<TService, TImplementation>(IocLifeStyle iocLifestyle)
            where TService : class
            where TImplementation : class, TService
        {
            RegisterIoc(typeof(TService), typeof(TImplementation), iocLifestyle);
        }

        public void RegisterIocR(Type serviceType, Type implementationType, IocLifeStyle iocLifestyle)
        {
            var implClassConstructors = implementationType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var implAssembly = implementationType.Assembly;
            if (implClassConstructors == null || implClassConstructors.Length > 0)
            {

                foreach (var implClassConstructor in implClassConstructors)
                {
                    var paras = implClassConstructor.GetParameters();

                    foreach (var para in paras)
                    {
                        var t = para.ParameterType;
                        if (t.IsInterface)
                        {
                            var reg = GetRegistration(t);
                            if (reg != null) continue;
                            var matchedClassType = implAssembly.GetExportedTypes().FirstOrDefault(et =>
                             et.IsClass && et.GetInterfaces() != null && et.GetInterfaces().FirstOrDefault(i1 => i1 == t) != null);
                            if (matchedClassType != null)
                            {
                                RegisterIocR(t, matchedClassType, iocLifestyle);
                            }
                            else
                            {
                                var allLoadedAssembly = System.AppDomain.CurrentDomain.GetAssemblies();

                                foreach (var currentAssembly in allLoadedAssembly)
                                {
                                    if (currentAssembly.IsDynamic || currentAssembly.GlobalAssemblyCache || currentAssembly.FullName.StartsWith("Microsoft", StringComparison.CurrentCultureIgnoreCase)) continue;
                                    matchedClassType = currentAssembly.GetExportedTypes().FirstOrDefault(et =>
                                            et.IsClass && et.GetInterfaces() != null && et.GetInterfaces().FirstOrDefault(i1 => i1 == t) != null);
                                    if (matchedClassType != null)
                                    {
                                        RegisterIocR(t, matchedClassType, iocLifestyle);
                                        break;
                                    }
                                }
                                if (matchedClassType == null)
                                {
                                    throw new InvalidOperationException(String.Format("Unable to register interface {0} in all assemblies while registering interface {1}", t.FullName, serviceType.FullName));
                                }
                            }
                        }
                    }

                }
            }

            RegisterIoc(serviceType, implementationType, iocLifestyle);

        }

        public void RegisterIocR<TService, TImplementation>(IocLifeStyle iocLifestyle)
            where TService : class
            where TImplementation : class, TService
        {
            RegisterIocR(typeof(TService), typeof(TImplementation), iocLifestyle);
        }
    }
    /// <summary>
    /// Ioc注册信息
    /// </summary>
    public class IocRegistration
    {
        /// <summary>
        /// 生命周期类型
        /// </summary>
        public IocLifeStyle LifeStyle { get; set; }
        /// <summary>
        /// 实现类型
        /// </summary>
        public Type ImplementationType { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        public Object Container { get; set; }
        /// <summary>
        /// 对象创建方法
        /// </summary>
        public Object ObjectCreator { get; set; }
    }
    public class IocFactory
    {
        private static IocHelper ioc = null;

        public static IIocHelper Instance
        {
            get
            {
                return CreateIocHelper();
            }
        }

        private static object _objLock = new object();

        public static IIocHelper CreateIocHelper(string IocType = "")
        {
            if (ioc == null)
            {
                lock (_objLock)
                {
                    if (ioc == null)
                    {
                        if (string.IsNullOrEmpty(IocType))
                        {
                            ioc = new IocHelper();
                        }
                    }
                }
            }
            return ioc;
        }
    }
    /// <summary>
    /// LifeStyle
    /// </summary>
    public enum IocLifeStyle : Int32
    {
        /// <summary>
        /// Scoped
        /// </summary>
        Scoped = 0,

        /// <summary>
        /// Singleton
        /// </summary>
        Singleton = 1,

        /// <summary>
        /// Transient
        /// </summary>
        Transient = 2
    }
    /// <summary>
    /// Ioc助手接口
    /// </summary>
    public interface IIocHelper : IDisposable
    {
        /// <summary>
        /// 容器
        /// </summary>
        object Container { get; }

        /// <summary>
        /// 注册接口
        /// </summary>
        /// <typeparam name="TService">接口类型</typeparam>
        /// <typeparam name="TImplementation">实现类型</typeparam>
        /// <param name="lifestyle">生命周期控制</param>
        void RegisterIoc<TService, TImplementation>(IocLifeStyle lifestyle)
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 根据类型进行接口注册
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        /// <param name="iocLifestyle">生命周期控制</param>
        void RegisterIoc(Type serviceType, Type implementationType, IocLifeStyle iocLifestyle);




        /// <summary>
        /// 根据接口类型进行注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="funcImpl">实现函数</param>
        /// <param name="iocLifestyle">生命周期控制</param>
        void RegisterIoc<TService>(Func<TService> funcImpl, IocLifeStyle iocLifestyle) where TService : class;


        /// <summary>
        /// 注册接口,如果接口依赖于其它接口，并且在实现类的汇编级中有相关实现，则会自动注册相关接口
        /// </summary>
        /// <typeparam name="TService">接口类型</typeparam>
        /// <typeparam name="TImplementation">实现类型</typeparam>
        /// <param name="lifestyle">生命周期控制</param>
        void RegisterIocR<TService, TImplementation>(IocLifeStyle lifestyle)
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 根据类型进行接口注册
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        /// <param name="iocLifestyle">生命周期控制</param>
        void RegisterIocR(Type serviceType, Type implementationType, IocLifeStyle iocLifestyle);


        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <returns>接口实例</returns>
        TInterface GetInstance<TInterface>()
            where TInterface : class;

        /// <summary>
        /// 获取类型注册信息
        /// </summary>
        /// <param name="interfaceTye">接口类型</param>
        /// <returns>注册信息</returns>
        IocRegistration GetRegistration(Type interfaceTye);
    }
    #endregion



}
