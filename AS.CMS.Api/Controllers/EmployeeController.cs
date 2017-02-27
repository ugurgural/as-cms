using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Collections.Generic;
using System.Web.Http;

namespace AS.CMS.Controllers
{
    [RoutePrefix("aday")]
    public class EmployeeController : ApiController
    {
        private IEmployeeService _employeeService;
        private IProfessionService _professionService;

        public EmployeeController(IEmployeeService employeeService, IProfessionService professionService)
        {
            _employeeService = employeeService;
            _professionService = professionService;
        }

        [Route("aday-listesi")]
        [HttpGet]
        public ApiResult List()
        {
            PageResultSet<Employee> activeEmployees = _employeeService.GetActiveEmployees(new PagingFilter());
            return new ApiResult() { Data = activeEmployees.PageData, Message = "OK", Success = true };
        }

        [Route("giris")]
        [HttpGet]
        public ApiResult Login(string mail, string password)
        {
            var currentEmployee = _employeeService.GetEmployeeWithMailAndPassword(mail, password);

            return new ApiResult() { Data = currentEmployee, Message = "OK", Success = currentEmployee != null };
        }

        [Route("yeni-aday")]
        [HttpGet]
        public ApiResult Register(Employee employeeEntity)
        {
            bool result = _employeeService.SaveEmployee(employeeEntity);

            return new ApiResult() { Data = null, Message = "OK", Success = true };
        }

        [Route("aday-kayit")]
        [HttpPost]
        public ApiResult SaveEmployee(Employee employeeEntity, EmployeeAvailability employeeAvailability, string[] employeeProfessions, string[] employeeWorkDays, string[] employeeWorkType)
        {
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

            return new ApiResult() { Data = null, Message = "OK", Success = true };
        }


    }
}