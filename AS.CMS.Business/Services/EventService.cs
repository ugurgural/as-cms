using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class EventService : IEventService
    {
        #region Repository Injection

        private IBaseRepository<Event> _eventRepository;

        public EventService(IBaseRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveEvent(Event eventEntity)
        {
            if (eventEntity.ID == 0)
            {
                _eventRepository.Create(eventEntity);
            }
            else
            {
                _eventRepository.Update(eventEntity);
            }

            return true;
        }

        public PageResultSet<Event> GetActiveEvents(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Event>("Name", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Event>("Name", pagingFilter.SearchText);

            return new PageResultSet<Event>()
            {
                PageData = _eventRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _eventRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Event GetEventWithID(int eventID)
        {
            return _eventRepository.GetById(eventID);
        }

        #endregion
    }
}