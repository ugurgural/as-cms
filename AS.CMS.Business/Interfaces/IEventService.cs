using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IEventService
    {
        bool SaveEvent(Event eventEntity);
        PageResultSet<Event> GetActiveEvents(PagingFilter pagingFilter);
        Event GetEventWithID(int eventID);
    }
}
