using System.Web.Mvc;
using System.Web.Routing;
using BookStore.Container;
using BookStore.Implementation;

namespace BookStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            NinjectContainer.RegisterModules(new MainModule());

            var bookSearchServcie = NinjectContainer.Resolve<IBookSearchService>();
            bookSearchServcie.InitializeIndex();
        }
    }
}
