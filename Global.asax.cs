using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using CryptocurrencyQuotes.Util;
using CryptocurrencyQuotes.Models;
using System.Data.Entity;

namespace CryptocurrencyQuotes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            Database.SetInitializer<ApplicationDbContext>(new AppDbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // DI
            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            kernel.Unbind<ModelValidatorProvider>();
        }
    }
}
