using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.CMS.Business.Interfaces
{
    public interface IRoleService
    {
        bool SaveRole(Role roleEntity);
        PageResultSet<Role> GetActiveRoles(PagingFilter pagingFilter);
        Role GetRoleWithID(int roleID);
    }
}
