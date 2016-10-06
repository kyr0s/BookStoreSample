using System;
using System.Web.Mvc;
using System.Web.Routing;
using BookStore.Container;
using BookStore.Implementation;
using NLog;

namespace BookStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            NinjectContainer.RegisterModules(new MainModule());

            var bookIndexBuilder = NinjectContainer.Resolve<IBookIndexBuilder>();
            bookIndexBuilder.Build();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            log.Error(exception);
        }

    }
}
