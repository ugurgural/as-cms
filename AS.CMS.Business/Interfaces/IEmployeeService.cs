using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IEmployeeService
    {
        bool SaveEmployee(Employee employeeEntity);
        PageResultSet<Employee> GetActiveEmployees(PagingFilter pagingFilter);
        Employee GetEmployeeWithID(int employeeID);
    }
}