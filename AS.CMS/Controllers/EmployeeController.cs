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
            List<string> selectedProfessions = new List<string>();

            if (employeeID.HasValue && employeeID.Value > 0)
            {
                currentEmployee = _employeeService.GetEmployeeWithID(employeeID.Value);
            }

            if (currentEmployee.Profession != null && currentEmployee.Profession.Count > 0)
            {
                for (int i = 0; i < currentEmployee.Profession.Count; i++)
                {
                    selectedProfessions.Add(currentEmployee.Profession[i].ID.ToString());
                }
            }

            string[] selectedAvailableDays = currentEmployee.EmployeeAvailability.Count > 0 ? currentEmployee.EmployeeAvailability[0].WorkDays.Split(',') : null;
            ViewBag.AvailableDaysSelectList = new MultiSelectList(EnumHelper.EnumToSelectList<WeekDay>(), "Text", "Value", selectedAvailableDays);

            string[] selectedWorkTypes = currentEmployee.EmployeeAvailability.Count > 0 ? currentEmployee.EmployeeAvailability[0].WorkType.Split(',') : null;
            ViewBag.WorkTypesSelectList = new MultiSelectList(EnumHelper.EnumToSelectList<WorkType>(), "Text", "Value", selectedWorkTypes);

            ViewBag.ProfessionsSelectList = new MultiSelectList(professionList, "ID", "Title", selectedProfessions.ToArray());
            return View(currentEmployee);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEmployee(Employee employeeEntity, EmployeeAvailability employeeAvailability, string[] employeeProfessions, string[] employeeWorkDays, string[] employeeWorkType)
        {
            if (!string.IsNullOrWhiteSpace(employeeEntity.Password))
            {
                employeeEntity.Password = UtilityHelper.GenerateMD5Hash(employeeEntity.Password);
            }

            if (employeeAvailability != null)
            {
                if (employeeWorkDays.Length > 0)
                {
                    employeeAvailability.WorkDays = string.Join(",", employeeWorkDays);
                }

                if (employeeWorkType.Length > 0)
                {
                    employeeAvailability.WorkType = string.Join(",", employeeWorkType);
                }

                if (employeeEntity.EmployeeAvailability == null || employeeEntity.EmployeeAvailability.Count == 0)
                {
                    employeeEntity.EmployeeAvailability = new List<EmployeeAvailability>();
                    employeeAvailability.Employee = employeeEntity;
                    employeeEntity.EmployeeAvailability.Add(employeeAvailability);
                }
                else
                {
                    employeeEntity.EmployeeAvailability[0] = employeeAvailability;
                }
            }

            List<Profession> professionList = new List<Profession>();

            if (employeeProfessions.Length > 0)
            {
                for (int i = 0; i < employeeProfessions.Length; i++)
                {
                    professionList.Add(_professionService.GetProfessionWithID(int.Parse(employeeProfessions[i])));
                }

                employeeEntity.Profession = professionList;
            }     
            
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

        [Route("resim-indir")]
        public FileResult DownLoad(string filePath, string fileList)
        {
            return File(UtilityHelper.GenerateZIPFileFromFileList(fileList.Split(','), filePath), "application/zip", filePath.Remove(0, filePath.LastIndexOf("/") + 1));
        }
    }
}