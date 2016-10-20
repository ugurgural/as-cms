using AS.CMS.Domain.Base;
using System.Collections.Generic;

namespace AS.CMS.Business.Interfaces
{
    public interface IModuleService
    {
        Module GetModuleFromPermalink(string permalink);

        IList<Module> GetActiveModules();
    }
}
