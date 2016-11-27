using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class EventProfessionQuotaService : IEventProfessionQuotaService
    {
        #region Repository Injection

        private IBaseRepository<EventProfessionQuota> _eventProfessionQuotaRepository;

        public EventProfessionQuotaService(IBaseRepository<EventProfessionQuota> eventProfessionQuotaRepository)
        {
            _eventProfessionQuotaRepository = eventProfessionQuotaRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveEventProfessionQuota(EventProfessionQuota eventProfessionQuotaEntity)
        {
            if (eventProfessionQuotaEntity.ID == 0)
            {
                _eventProfessionQuotaRepository.Create(eventProfessionQuotaEntity);
            }
            else
            {
                _eventProfessionQuotaRepository.Update(eventProfessionQuotaEntity);
            }

            return true;
        }

        public PageResultSet<EventProfessionQuota> GetActiveEventProfessionQuotas(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<EventProfessionQuota>("ID", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<EventProfessionQuota>("ID", pagingFilter.SearchText);

            return new PageResultSet<EventProfessionQuota>()
            {
                PageData = _eventProfessionQuotaRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _eventProfessionQuotaRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public EventProfessionQuota GetEventProfessionQuotaWithID(int eventProfessionQuotaID)
        {
            return _eventProfessionQuotaRepository.GetById(eventProfessionQuotaID);
        }

        #endregion
    }
}