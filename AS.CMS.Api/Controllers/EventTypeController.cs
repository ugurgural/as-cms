using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Web.Http;

namespace AS.CMS.Controllers
{
    [RoutePrefix("api/etkinlik")]
    public class EventTypeController : ApiController
    {
        private IEventTypeService _eventTypeService;

        public EventTypeController(IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
        }

        [HttpGet]
        [Route("etkinlik-tipleri-listesi")]
        public ApiResult List()
        {
            PageResultSet<EventType> activeEventTypes = _eventTypeService.GetActiveEventTypes(new PagingFilter());

            return new ApiResult() { Data = activeEventTypes.PageData, Message = "OK", Success = true };
        }

        [Route("etkinlik-tipi/{eventTypeID}")]
        public ApiResult Get(int eventTypeID)
        {
            return new ApiResult() { Data = _eventTypeService.GetEventTypeWithID(eventTypeID), Message = "OK", Success = true };
        }

        [HttpPost]
        [Route("etkinlik-tipi-kayit")]
        public ApiResult SaveEventType(EventType eventTypeEntity)
        {
            bool result = _eventTypeService.SaveEventType(eventTypeEntity);

            return new ApiResult() { Data = null, Message = "OK", Success = result };
        }
    }
}