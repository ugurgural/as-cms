using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IProfessionService
    {
        bool SaveProfession(Profession professionEntity);
        PageResultSet<Profession> GetActiveProfessions(PagingFilter pagingFilter);
        Profession GetProfessionWithID(int professionID);
    }
}