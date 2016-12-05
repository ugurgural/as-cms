using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("aday")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class EmployeeController : BaseController
    {
        private IEmployeeService _employeeService;
        private IProfessionService _professionService;

        public EmployeeController(IEmployeeService employeeService, IProfessionService professionService, IModuleService moduleService) : base(moduleService)
        {
            _employeeService = employeeService;
            _professionService = professionService;
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
            IList<Profession> professionList = _professionService.GetActiveProfessions(new PagingFilter()).PageData;
            int selectedProfessionID = 0;

            if (employeeID.HasValue && employeeID.Value > 0)
            {
                currentEmployee = _employeeService.GetEmployeeWithID(employeeID.Value);
            }

            if (currentEmployee.Profession != null && currentEmployee.Profession.Count > 0)
            {
                selectedProfessionID = currentEmployee.Profession[0].ID;
            }

            string[] selectedAvailableDays = currentEmployee.EmployeeAvailability.Count > 0 ? currentEmployee.EmployeeAvailability[0].WorkDays.Split(',') : null;
            ViewBag.AvailableDaysSelectList = new MultiSelectList(EnumHelper.EnumToSelectList<WeekDay>(), "Text", "Value", selectedAvailableDays);

            string[] selectedWorkTypes = currentEmployee.EmployeeAvailability.Count > 0 ? currentEmployee.EmployeeAvailability[0].WorkType.Split(',') : null;
            ViewBag.WorkTypesSelectList = new MultiSelectList(EnumHelper.EnumToSelectList<WorkType>(), "Text", "Value", selectedWorkTypes);

            ViewBag.ProfessionsSelectList = new SelectList(professionList, "ID", "Title", selectedProfessionID);
            return View(currentEmployee);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEmployee(Employee employeeEntity, EmployeeAvailability employeeAvailability, int professionID, string[] employeeWorkDays, string[] employeeWorkType)
        {
            if (employeeAvailability != null)
            {
                if (employeeEntity.EmployeeAvailability == null)
                {
                    employeeEntity.EmployeeAvailability = new List<EmployeeAvailability>();
                    employeeAvailability.Employee = employeeEntity;
                    employeeEntity.EmployeeAvailability.Add(employeeAvailability);
                }

                if (employeeWorkDays.Length > 0)
                {
                    employeeAvailability.WorkDays = string.Join(",", employeeWorkDays);
                }

                if (employeeWorkType.Length > 0)
                {
                    employeeAvailability.WorkType = string.Join(",", employeeWorkType);
                }
            }

            List<Profession> professionList = new List<Profession>();
            professionList.Add(_professionService.GetProfessionWithID(professionID));
            employeeEntity.Profession = professionList;
            
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