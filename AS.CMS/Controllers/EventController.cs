using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using AS.CMS.Business.Helpers;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class EventController : BaseController
    {
        private IEventService _eventService;
        private IEventTypeService _eventTypeService;

        public EventController(IEventService eventService, IEventTypeService eventTypeService, IModuleService moduleService) : base(moduleService)
        {
            _eventService = eventService;
            _eventTypeService = eventTypeService;
        }

        [Route("etkinlik-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<Event> activeEvents = _eventService.GetActiveEvents(pageFilter);
            pageFilter.PlaceHolderText = "İsimlerde ara";
            SetPageFilters(pageFilter, activeEvents.Count);

            return View(activeEvents.PageData);
        }

        [Route("yeni-etkinlik-ekle")]
        public ActionResult AddOrEdit(int? eventID)
        {
            Event currentEvent = new Event();

            if (eventID.HasValue && eventID.Value > 0)
            {
                currentEvent = _eventService.GetEventWithID(eventID.Value);
            }

            IList<EventType> eventTypeList = _eventTypeService.GetActiveEventTypes(new PagingFilter()).PageData;
            int selectedEventTypeID = currentEvent.EventType != null ? currentEvent.EventType.ID : 0;
            ViewBag.EventTypeSelectList = new SelectList(eventTypeList, "ID", "Name", selectedEventTypeID);

            string[] selectedEventDays = !string.IsNullOrWhiteSpace(currentEvent.EventDays) ? currentEvent.EventDays.Split(',') : new string[0];
            ViewBag.EventDaysSelectList = new MultiSelectList(EnumHelper.EnumToSelectList<WeekDay>(), "Text", "Value", selectedEventDays);

            return View(currentEvent);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEvent(Event eventEntity, int eventTypeID, string[] eventDaysList)
        {
            eventEntity.EventType = new EventType() { ID = eventTypeID };

            if (eventDaysList.Length > 0)
            {
                eventEntity.EventDays = string.Join(",", eventDaysList);
            }

            bool result = _eventService.SaveEvent(eventEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("etkinlik-listesi", "etkinlik");
        }
    }
}