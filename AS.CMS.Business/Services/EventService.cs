using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AS.CMS.Business.Services
{
    public class EventService : IEventService
    {
        #region Repository Injection

        private IBaseRepository<Event> _eventRepository;
        private IBaseRepository<EventEmployee> _eventEmployeeRepository;

        public EventService(IBaseRepository<Event> eventRepository, IBaseRepository<EventEmployee> eventEmployeeRepository)
        {
            _eventRepository = eventRepository;
            _eventEmployeeRepository = eventEmployeeRepository;
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

        public bool SaveEventEmployee(EventEmployee eventEmployeeEntity)
        {
            if (eventEmployeeEntity.ID == 0)
            {
                _eventEmployeeRepository.Create(eventEmployeeEntity);
            }
            else
            {
                _eventEmployeeRepository.Update(eventEmployeeEntity);
            }

            return true;
        }

        public IList<EventEmployee> GetActiveEventEmployees(int eventID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Event.ID", eventID));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Where(x => x.Event.IsActive == true).ToList();
        }

        public IList<EventEmployee> GetApprovalEventEmployees(int eventID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Event.ID", eventID));
            defaultCriteria.Add(Expression.Eq("IsActive", false));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Where(x => x.Event.IsActive == true).ToList();
        }

        public IList<EventEmployee> GetActiveEmployeeEvents(int employeeID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Employee.ID", employeeID));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Where(x => x.Event.IsActive == true).ToList();
        }

        public IList<Event> GetEventWithEmployeeID(int employeeID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Employee.ID", employeeID));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Where(x => x.Event.IsActive == true).Select(x => x.Event).ToList();
        }

        public IList<EventEmployee> GetEventEmployeeWithID(int employeeID, int eventID, bool isActive = true)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Employee.ID", employeeID));
            defaultCriteria.Add(Expression.Eq("Event.ID", eventID));
            defaultCriteria.Add(Expression.Eq("IsActive", isActive));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria);
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

        public IList<Event> GetPastEvents()
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<Event>();
            defaultCriteria.Add(Expression.Lt("EndDate", DateTime.Now));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventRepository.GetWithCriteria(defaultCriteria);
        }

        public EventCount GetEventCounts()
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));
            IList<EventEmployee> eventCountList = _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Take(5).ToList();

            var nameGroups = eventCountList
                            .GroupBy(x => x.Event.Name)
                            .Select(x => new
                                {
                                    EventName = x.Key,
                                    EventCount = x.Count()
                                })
                            .OrderByDescending(x => x.EventCount).ToArray();

            return new EventCount() {
                EventName = nameGroups.Select(x => x.EventName).ToArray(),
                Count = nameGroups.Select(x => x.EventCount).ToArray() };
        }

        public Event GetEventWithID(int eventID)
        {
            return _eventRepository.GetById(eventID);
        }

        #endregion
    }
}