using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IEventTypeService
    {
        bool SaveEventType(EventType eventTypeEntity);
        PageResultSet<EventType> GetActiveEventTypes(PagingFilter pagingFilter);
        EventType GetEventTypeWithID(int eventTypeID);
    }
}
