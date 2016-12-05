using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using System.Collections.Generic;

namespace AS.CMS.Business.Interfaces
{
    public interface IEventService
    {
        bool SaveEvent(Event eventEntity);
        PageResultSet<Event> GetActiveEvents(PagingFilter pagingFilter);
        IList<EventEmployee> GetActiveEventEmployees(int eventID);
        IList<EventEmployee> GetActiveEmployeeEvents(int employeeID);
        IList<EventEmployee> GetEventEmployeeWithID(int employeeID, int eventID);
        bool SaveEventEmployee(EventEmployee eventEmployeeEntity);
        Event GetEventWithID(int eventID);
    }
}