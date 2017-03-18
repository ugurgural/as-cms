using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Web.Http;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik-kota")]
    public class EventProfessionQuotaController : ApiController
    {
        private IEventProfessionQuotaService _eventProfessionQuotaService;
        private IEventService _eventService;
        private IProfessionService _professionService;

        public EventProfessionQuotaController(IEventProfessionQuotaService eventProfessionQuotaService, IEventService eventService, IProfessionService professionService)
        {
            _eventProfessionQuotaService = eventProfessionQuotaService;
            _eventService = eventService;
            _professionService = professionService;
        }

        [Route("etkinlik-kota-listesi")]
        [HttpGet]
        public ApiResult Index()
        {
            PageResultSet<EventProfessionQuota> activeEventProfessionQuotas = _eventProfessionQuotaService.GetActiveEventProfessionQuotas(new PagingFilter());

            return new ApiResult() { Data = activeEventProfessionQuotas.PageData, Message = "OK", Success = true };
        }

        [Route("etkinlik-kota/{eventID}")]
        public ApiResult GetProfessionQuotaWithEventID(int eventID)
        {
            return new ApiResult() { Data = _eventProfessionQuotaService.GetEventProfessionQuotaWithEventID(eventID), Message = "OK", Success = true };
        }

        [HttpPost]
        [Route("etkinlik-kota-kayit")]
        public ApiResult SaveEventProfessionQuota(EventProfessionQuota eventProfessionQuotaEntity, int eventID, int professionID)
        {
            eventProfessionQuotaEntity.Event = new Event() { ID = eventID };
            eventProfessionQuotaEntity.Profession = new Profession() { ID = professionID };

            bool result = _eventProfessionQuotaService.SaveEventProfessionQuota(eventProfessionQuotaEntity);

            return new ApiResult() { Data = null, Message = "OK", Success = result };
        }
    }
}