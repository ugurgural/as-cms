using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Collections.Generic;

namespace AS.CMS.Business.Interfaces
{
    public interface IEventService
    {
        bool SaveEvent(Event eventEntity);
        PageResultSet<Event> GetActiveEvents(PagingFilter pagingFilter);
        IList<Event> GetPastEvents();
        IList<EventEmployee> GetActiveEventEmployees(int eventID);
        IList<EventEmployee> GetActiveEmployeeEvents(int employeeID);
        IList<Event> GetEventWithEmployeeID(int employeeID);
        IList<EventEmployee> GetEventEmployeeWithID(int employeeID, int eventID);
        bool SaveEventEmployee(EventEmployee eventEmployeeEntity);
        Event GetEventWithID(int eventID);
        EventCount GetEventCounts();
    }
}