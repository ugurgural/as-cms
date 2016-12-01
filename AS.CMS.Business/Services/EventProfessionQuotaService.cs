using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;
using System.Collections.Generic;

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
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventProfessionQuota>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));
            defaultCriteria.Add(Expression.Eq("Event.ID", int.Parse(pagingFilter.SearchText)));

            DetachedCriteria rowCountcriteria = defaultCriteria;
            DetachedCriteria activeContentPagingcriteria = defaultCriteria;

            return new PageResultSet<EventProfessionQuota>()
            {
                PageData = _eventProfessionQuotaRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter),
                Count = _eventProfessionQuotaRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public IList<EventProfessionQuota> GetEventProfessionQuotaWithProfessionID(int eventID, int professionID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventProfessionQuota>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));
            defaultCriteria.Add(Expression.Eq("Event.ID", eventID));
            defaultCriteria.Add(Expression.Eq("Profession.ID", professionID));

            return _eventProfessionQuotaRepository.GetWithCriteria(defaultCriteria);
        }

        public EventProfessionQuota GetEventProfessionQuotaWithID(int eventProfessionQuotaID)
        {
            return _eventProfessionQuotaRepository.GetById(eventProfessionQuotaID);
        }

        #endregion
    }
}