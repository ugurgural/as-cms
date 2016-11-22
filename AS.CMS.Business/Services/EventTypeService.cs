using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class EventTypeService : IEventTypeService
    {
        #region Repository Injection

        private IBaseRepository<EventType> _eventTypeRepository;

        public EventTypeService(IBaseRepository<EventType> eventTypeRepository)
        {
            _eventTypeRepository = eventTypeRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveEventType(EventType eventTypeEntity)
        {
            if (eventTypeEntity.ID == 0)
            {
                _eventTypeRepository.Create(eventTypeEntity);
            }
            else
            {
                _eventTypeRepository.Update(eventTypeEntity);
            }

            return true;
        }

        public PageResultSet<EventType> GetActiveEventTypes(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<EventType>("Name", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<EventType>("Name", pagingFilter.SearchText);

            return new PageResultSet<EventType>()
            {
                PageData = _eventTypeRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _eventTypeRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public EventType GetEventTypeWithID(int eventTypeID)
        {
            return _eventTypeRepository.GetById(eventTypeID);
        }

        #endregion
    }
}