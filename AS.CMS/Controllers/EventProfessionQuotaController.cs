using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik-kota")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class EventProfessionQuotaController : BaseController
    {
        private IEventProfessionQuotaService _eventProfessionQuotaService;
        private IEventService _eventService;
        private IProfessionService _professionService;

        public EventProfessionQuotaController(IEventProfessionQuotaService eventProfessionQuotaService, IEventService eventService, IProfessionService professionService, IModuleService moduleService) : base(moduleService)
        {
            _eventProfessionQuotaService = eventProfessionQuotaService;
            _eventService = eventService;
            _professionService = professionService;
        }

        [Route("etkinlik-kota-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["eventID"]))
            {
                pageFilter.SearchText = Request.QueryString["eventID"];
            }

            PageResultSet<EventProfessionQuota> activeEventProfessionQuotas = _eventProfessionQuotaService.GetActiveEventProfessionQuotas(pageFilter);
            pageFilter.PlaceHolderText = "İsimlerde ara";
            SetPageFilters(pageFilter, activeEventProfessionQuotas.Count);

            return View(activeEventProfessionQuotas.PageData);
        }

        [Route("yeni-etkinlik-kota-ekle")]
        public ActionResult AddOrEdit(int? eventProfessionQuotaID)
        {
            EventProfessionQuota currentEventProfessionQuota = new EventProfessionQuota();

            if (eventProfessionQuotaID.HasValue && eventProfessionQuotaID.Value > 0)
            {
                currentEventProfessionQuota = _eventProfessionQuotaService.GetEventProfessionQuotaWithID(eventProfessionQuotaID.Value);
            }

            IList<Event> eventList = _eventService.GetActiveEvents(new PagingFilter()).PageData;
            int selectedEventID = currentEventProfessionQuota.Event != null ? currentEventProfessionQuota.Event.ID : 0;
            ViewBag.EventSelectList = new SelectList(eventList, "ID", "Name", selectedEventID);

            IList<Profession> professionList = _professionService.GetActiveProfessions(new PagingFilter()).PageData;
            int selectedProfessionID = currentEventProfessionQuota.Profession != null ? currentEventProfessionQuota.Profession.ID : 0;
            ViewBag.ProfessionSelectList = new SelectList(eventList, "ID", "Title", selectedProfessionID);

            return View(currentEventProfessionQuota);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEventProfessionQuota(EventProfessionQuota eventProfessionQuotaEntity, int eventID, int professionID)
        {
            eventProfessionQuotaEntity.Event = new Event() { ID = eventID };
            eventProfessionQuotaEntity.Profession = new Profession() { ID = professionID };

            bool result = _eventProfessionQuotaService.SaveEventProfessionQuota(eventProfessionQuotaEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("etkinlik-kota-listesi", "etkinlik-kota");
        }
    }
}