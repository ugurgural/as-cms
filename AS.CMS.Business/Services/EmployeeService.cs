using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        #region Repository Injection

        private IBaseRepository<Employee> _employeeRepository;

        public EmployeeService(IBaseRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveEmployee(Employee employeeEntity)
        {
            foreach (var jobExperienceItem in employeeEntity.EmployeeJobExperience)
            {
                jobExperienceItem.Employee = employeeEntity;
            }

            foreach (var educationItem in employeeEntity.EmployeeEducation)
            {
                educationItem.Employee = employeeEntity;
            }

            foreach (var certificateItem in employeeEntity.EmployeeCertificateAndLanguage)
            {
                certificateItem.Employee = employeeEntity;
            }

            foreach (var availabilityItem in employeeEntity.EmployeeAvailability)
            {
                availabilityItem.Employee = employeeEntity;
            }

            if (employeeEntity.ID == 0)
            {
                _employeeRepository.Create(employeeEntity);
            }
            else
            {
                _employeeRepository.Update(employeeEntity);
            }

            return true;
        }

        public PageResultSet<Employee> GetActiveEmployees(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Employee>("FirstName", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Employee>("FirstName", pagingFilter.SearchText);

            return new PageResultSet<Employee>()
            {
                PageData = _employeeRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _employeeRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Employee GetEmployeeWithID(int employeeID)
        {
            return _employeeRepository.GetById(employeeID);
        }

        #endregion
    }
}