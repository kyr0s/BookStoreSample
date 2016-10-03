using System.Web.Mvc;
using BookStore.Models.Home;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookListViewModelBuilder bookListViewModelBuilder;

        public HomeController(IBookListViewModelBuilder bookListViewModelBuilder)
        {
            this.bookListViewModelBuilder = bookListViewModelBuilder;
        }

        public ActionResult Index(string query)
        {
            var model = bookListViewModelBuilder.Build(query);
            return View(model);
        }
    }
}