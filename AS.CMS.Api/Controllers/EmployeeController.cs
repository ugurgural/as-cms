using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (currentEmployee != null)
            {
                currentEmployee.EmployeeAvailability = currentEmployee.EmployeeAvailability.Where(x => x.IsActive == true).ToList();
                currentEmployee.EmployeeCertificateAndLanguage = currentEmployee.EmployeeCertificateAndLanguage.Where(x => x.IsActive == true).ToList();
                currentEmployee.EmployeeEducation = currentEmployee.EmployeeEducation.Where(x => x.IsActive == true).ToList();
                currentEmployee.EmployeeJobExperience = currentEmployee.EmployeeJobExperience.Where(x => x.IsActive == true).ToList();
            }

            return new ApiResult() { Data = currentEmployee, Message = "OK", Success = currentEmployee != null };
        }

        [Route("yeni-aday")]
        [HttpPost]
        public ApiResult Register(Employee employeeEntity)
        {
            ApiResult result = null;
            employeeEntity.BirthDate = employeeEntity.ID > 0 ? employeeEntity.BirthDate : DateTime.Now;

            if (!string.IsNullOrWhiteSpace(employeeEntity.Password))
            {

                employeeEntity.Password = UtilityHelper.GenerateMD5Hash(employeeEntity.Password);
            }
            else
            {
                employeeEntity.Password = _employeeService.GetEmployeeWithID(employeeEntity.ID).Password;
            }


            if (employeeEntity.ID == 0 && _employeeService.GetEmployeeWithMail(employeeEntity.MailAddress) != null)
            {
                result = new ApiResult() { Data = null, Message = "Mevcut Mail Adresi İle Kayıtlı Bir Üye Sistemde Mevcut, Lütfen Bilgilerinizi Kontrol Ediniz!", Success = false };
            }
            else
            {
                _employeeService.SaveEmployee(employeeEntity);
                result = new ApiResult() { Data = null, Message = "OK", Success = true };
            }

            return result;
        }

        [HttpGet]
        [Route("yeni-sifre")]
        public ApiResult LostPassword(string mail)
        {
            ApiResult result = null;

            if (_employeeService.GetEmployeeWithMail(mail) != null)
            {
                UtilityHelper.SendMail(mail, "Yeni Üyelik Şifreniz", string.Format("Şifrenizi sıfırlama talebinde bulundunuz, aşağıdaki linkten şifrenizi sıfırlayabilirsiniz.<br /><br />{0}", Request.RequestUri.GetLeftPart(UriPartial.Authority).ToString() + "/hesap/yeni-sifre?mail=" + mail));
                result = new ApiResult() { Data = null, Message = "Şifre sıfırlama talebiniz alındı. Lütfen belirttiğiniz mail adresini kontrol edin.", Success = true };
            }

            return result;
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