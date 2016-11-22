using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class EventTypeController : BaseController
    {
        private IEventTypeService _eventTypeService;

        public EventTypeController(IEventTypeService eventTypeService, IModuleService moduleService) : base(moduleService)
        {
            _eventTypeService = eventTypeService;
        }

        [Route("etkinlik-tipleri-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<EventType> activeEventTypes = _eventTypeService.GetActiveEventTypes(pageFilter);
            pageFilter.PlaceHolderText = "İsimlerde ara";
            SetPageFilters(pageFilter, activeEventTypes.Count);

            return View(activeEventTypes.PageData);
        }

        [Route("yeni-etkinlik-tipi-ekle")]
        public ActionResult AddOrEdit(int? eventTypeID)
        {
            EventType currentEventType = new EventType();

            if (eventTypeID.HasValue && eventTypeID.Value > 0)
            {
                currentEventType = _eventTypeService.GetEventTypeWithID(eventTypeID.Value);
            }

            return View(currentEventType);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEventType(EventType eventTypeEntity)
        {
            bool result = _eventTypeService.SaveEventType(eventTypeEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("etkinlik-tipleri-listesi", "etkinlik");
        }
    }
}