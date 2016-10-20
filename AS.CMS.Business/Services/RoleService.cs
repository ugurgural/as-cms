using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace AS.CMS.Business.Services
{
    public class RoleService : IRoleService
    {
        #region Repository Injection

        private IBaseRepository<Role> _roleRepository;

        public RoleService(IBaseRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveRole(Role roleEntity)
        {
            if (roleEntity.ID == 0)
            {
                _roleRepository.Create(roleEntity);
            }
            else
            {
                _roleRepository.Update(roleEntity);
            }

            return true;
        }

        public PageResultSet<Role> GetActiveRoles(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Content>("Name", pagingFilter.SearchText);
            DetachedCriteria activeRolePagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Content>("Name", pagingFilter.SearchText);

            return new PageResultSet<Role>()
            {
                PageData = _roleRepository.GetWithCriteriaByPaging(activeRolePagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _roleRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Role GetRoleWithID(int roleID)
        {
            return _roleRepository.GetById(roleID);
        }

        #endregion
    }
}
