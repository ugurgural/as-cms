using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik-kota")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class EventProfessionQuotaController : BaseController
    {
        private IEventProfessionQuotaService _eventProfessionQuotaService;

        public EventProfessionQuotaController(IEventProfessionQuotaService eventProfessionQuotaService, IModuleService moduleService) : base(moduleService)
        {
            _eventProfessionQuotaService = eventProfessionQuotaService;
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
            EventProfessionQuota currentEventType = new EventProfessionQuota();

            if (eventProfessionQuotaID.HasValue && eventProfessionQuotaID.Value > 0)
            {
                currentEventType = _eventProfessionQuotaService.GetEventProfessionQuotaWithID(eventProfessionQuotaID.Value);
            }

            return View(currentEventType);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEventProfessionQuota(EventProfessionQuota eventProfessionQuotaEntity)
        {
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