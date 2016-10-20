using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Linq;

namespace AS.CMS.Business.Services
{
    public class ModuleService : IModuleService
    {
        #region Repository Injection

        private IBaseRepository<Module> _moduleRepository;

        public ModuleService(IBaseRepository<Module> moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        #endregion

        #region Controller Based Methods

        public IList<Module> GetActiveModules()
        {
            DetachedCriteria activeModulecriteria = CriteriaHelper.GetDefaultCriteria<Module>();
            return _moduleRepository.GetWithCriteria(activeModulecriteria, CriteriaHelper.GetDefaultOrder());
        }

        #endregion

        #region Helper Methods

        public Module GetModuleFromPermalink(string permalink)
        {
            DetachedCriteria activeModulecriteria = CriteriaHelper.GetDefaultSearchCriteria<Module>("FriendlyName", permalink);
            return _moduleRepository.GetWithCriteria(activeModulecriteria, CriteriaHelper.GetDefaultOrder()).FirstOrDefault();
        }

        #endregion
    }
}