using Autofac;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace SPText.Unity
{
    public class ContainnerFactory
    {
        private static IUnityContainer container = null;
        /// <summary>
        ///单例模式
        /// </summary>
        static ContainnerFactory()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Unity\\Unity.Config");
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
            IUnityContainer _container = new UnityContainer();
            section.Configure(_container, "zidingyi");
            container = _container;
        }

        public static IUnityContainer GetContainer()
        {
            return container;
        }
    }
}
