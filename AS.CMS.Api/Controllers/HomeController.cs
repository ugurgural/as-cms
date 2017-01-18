using System.Collections.Generic;
using System.Web.Http;

namespace AS.CMS.Api.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public List<string> Index()
        {
            return new List<string>() { "mehmet", "ugur", "gural" };
        }
    }
}