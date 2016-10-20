using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("aday")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class EmployeeController : BaseController
    {
        private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, IModuleService moduleService) : base(moduleService)
        {
            _employeeService = employeeService;
        }

        [Route("aday-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<Employee> activeEmployees = _employeeService.GetActiveEmployees(pageFilter);
            pageFilter.PlaceHolderText = "İsimlerde ara";
            SetPageFilters(pageFilter, activeEmployees.Count);

            return View(activeEmployees.PageData);
        }

        [Route("yeni-aday-ekle")]
        public ActionResult AddOrEdit(int? employeeID)
        {
            Employee currentEmployee = new Employee();

            if (employeeID.HasValue && employeeID.Value > 0)
            {
                currentEmployee = _employeeService.GetEmployeeWithID(employeeID.Value);
            }

            return View(currentEmployee);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEmployee(Employee employeeEntity, string picture, string galleryImages)
        {
            employeeEntity.Picture = picture;
            employeeEntity.CVFile = galleryImages;
            bool result = _employeeService.SaveEmployee(employeeEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("aday-listesi", "aday");
        }
    }
}