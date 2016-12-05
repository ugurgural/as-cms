using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IEmployeeService
    {
        bool SaveEmployee(Employee employeeEntity);
        PageResultSet<Employee> GetActiveEmployees(PagingFilter pagingFilter);
        PageResultSet<Employee> GetActiveEmployeesFromSearch(Employee employeeSearchCriteria, PagingFilter pagingFilter);
        Employee GetEmployeeWithID(int employeeID);
    }
}