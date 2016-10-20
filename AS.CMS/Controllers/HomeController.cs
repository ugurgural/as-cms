using AS.CMS.Business.Interfaces;
using System.Web.Mvc;
using System.Linq;
using AS.CMS.Domain.Common;

namespace AS.CMS.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private INewsService _newsService;

        public HomeController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public ActionResult Index()
        {
            return View(_newsService.GetActiveNews(new PagingFilter()).PageData.Take(5).ToList());
        }
    }
}