using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class ProfessionService : IProfessionService
    {
        #region Repository Injection

        private IBaseRepository<Profession> _professionRepository;

        public ProfessionService(IBaseRepository<Profession> professionRepository)
        {
            _professionRepository = professionRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveProfession(Profession professionEntity)
        {
            if (professionEntity.ID == 0)
            {
                _professionRepository.Create(professionEntity);
            }
            else
            {
                _professionRepository.Update(professionEntity);
            }

            return true;
        }

        public PageResultSet<Profession> GetActiveProfessions(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Profession>("Name", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Profession>("Name", pagingFilter.SearchText);

            return new PageResultSet<Profession>()
            {
                PageData = _professionRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _professionRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Profession GetProfessionWithID(int professionID)
        {
            return _professionRepository.GetById(professionID);
        }

        #endregion
    }
}