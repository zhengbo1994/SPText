using SPMVCText.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SPMVCText
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //����autofac��������ע��Ϳ��Ʒ�ת����
            //AutoFacConfig.Register();


            #region  EFԤ�ȣ�EF����������
            //using (var dbcontext = new BingFaTestEntities())
            //{
            //    var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
            //    var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            //    mappingCollection.GenerateViews(new List<EdmSchemaError>());
            //}
            #endregion
        }
    }
}
