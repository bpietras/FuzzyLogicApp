using System;
using System.Web.Mvc;
using System.Web.Routing;
using FuzzyLogicWebService.Controllers;
using Ninject;
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ILogger>().To<NLogLogger>();
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(ninjectKernel));
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}