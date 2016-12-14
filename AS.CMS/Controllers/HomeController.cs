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
        private IEventService _eventService;
        private IEmployeeService _employeeService;
        private IProfessionService _professionService;
        private IAnnouncementService _announcementService;

        public HomeController(INewsService newsService, IEventService eventService, IEmployeeService employeeService, IProfessionService professionService, IAnnouncementService announcementService)
        {
            _newsService = newsService;
            _eventService = eventService;
            _employeeService = employeeService;
            _professionService = professionService;
            _announcementService = announcementService;
        }

        public ActionResult Index()
        {
            ViewBag.EmployeeCount = _employeeService.GetActiveEmployees(new PagingFilter()).Count;
            ViewBag.EmployeeMaleCount = _employeeService.GetEmployeeCountByGender(GenderType.Male);
            ViewBag.EmployeeFemaleCount = _employeeService.GetEmployeeCountByGender(GenderType.Female);
            ViewBag.EventCount = _eventService.GetActiveEvents(new PagingFilter()).Count;
            ViewBag.ProfessionCount = _professionService.GetActiveProfessions(new PagingFilter()).Count;
            ViewBag.AnnouncementCount = _announcementService.GetActiveAnnouncements(new PagingFilter()).Count;

            ViewBag.HighestEventNames = _eventService.GetEventCounts();

            return View(_eventService.GetActiveEvents(new PagingFilter()).PageData.Take(5).ToList());
        }
    }
}