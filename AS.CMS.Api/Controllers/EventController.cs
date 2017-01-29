using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik")]
    public class EventController : Controller
    {
        private IEventService _eventService;
        private IEventTypeService _eventTypeService;
        private IEventProfessionQuotaService _eventProfessionQuotaService;
        private IEmployeeService _employeeService;

        public EventController(IEventService eventService, IEventTypeService eventTypeService, IEmployeeService employeeService, IEventProfessionQuotaService eventProfessionQuotaService)
        {
            _eventService = eventService;
            _eventTypeService = eventTypeService;
            _eventProfessionQuotaService = eventProfessionQuotaService;
            _employeeService = employeeService;
        }

        [Route("etkinlik-listesi")]
        [HttpGet]
        public IList<Event> List(PagingFilter pageFilter)
        {
            PageResultSet<Event> activeEvents = _eventService.GetActiveEvents(pageFilter);
            return activeEvents.PageData;
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEventEmployee(int eventID, int employeeID, int professionID)
        {
            Employee currentEmployee = _employeeService.GetEmployeeWithID(employeeID);
            Event currentEvent = _eventService.GetEventWithID(eventID);

            int eventProfessionQuotaCount = _eventProfessionQuotaService.GetEventProfessionQuotaWithProfessionID(eventID, professionID).Where(x => x.Gender == currentEmployee.Gender).Count();

            int employeeProfessionCountForCurrentEvent = _eventService.GetActiveEventEmployees(eventID)
                                                         .Where(x => x.Employee.Profession.Where(y => y.ID == professionID).Any() &&                                    x.Employee.Gender == currentEmployee.Gender).Count();

            var employeeActiveEvents = _eventService.GetActiveEmployeeEvents(employeeID);

            if (string.IsNullOrWhiteSpace(currentEvent.EventDays))
            {
                SetModalStatusMessage(ModalStatus.Error, "Etkinlik için herhangi bir çalışma günü belirlenmemiş. Lütfen kontrol ediniz !");
                return RedirectToAction("etkinlik-listesi", "etkinlik");
            }

            if (!string.IsNullOrWhiteSpace(currentEmployee.EmployeeAvailability[0].WorkDays))
            {
                string[] employeeWorkDays = currentEmployee.EmployeeAvailability[0].WorkDays.Split(',');
                string[] eventWorkDays = currentEvent.EventDays.Split(',');

                for (int i = 0; i < eventWorkDays.Length; i++)
                {
                    if (!employeeWorkDays.Contains(eventWorkDays[i]))
                    {
                        SetModalStatusMessage(ModalStatus.Error, "Seçilen adayın çalışma günleri etkinlik günü ile örtüşmemektedir. Lütfen kontrol ediniz !");
                        return RedirectToAction("etkinlik-listesi", "etkinlik");
                    }
                }
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error, "Seçilen aday uygun olduğu herhangi bir çalışma günü belirlememiş. Lütfen kontrol ediniz !");
                return RedirectToAction("etkinlik-listesi", "etkinlik");
            }

            if (employeeActiveEvents != null && employeeActiveEvents.Count > 0)
            {
                foreach (var employeeEventItem in employeeActiveEvents)
                {
                    if (employeeEventItem.Event.ID.Equals(currentEvent.ID))
                    {
                        SetModalStatusMessage(ModalStatus.Error, "Seçilen aday bu etkinliğe zaten eklenmiş. Lütfen kontrol ediniz !");
                        return RedirectToAction("etkinlik-listesi", "etkinlik");
                    }

                    string[] employeeEventWorkDays = employeeEventItem.Event.EventDays.Split(',');
                    string[] currentEventWorkDays = currentEvent.EventDays.Split(',');

                    for (int i = 0; i < employeeEventWorkDays.Length; i++)
                    {
                        if (currentEventWorkDays.Contains(employeeEventWorkDays[i]))
                        {
                            SetModalStatusMessage(ModalStatus.Error, "Seçilen adayın etkinliğin olduğu günlerde katıldığı başka bir etkinlik bulunmakta. Lütfen kontrol ediniz !");
                            return RedirectToAction("etkinlik-listesi", "etkinlik");
                        }
                    }
                }
            }

            if (employeeProfessionCountForCurrentEvent >= eventProfessionQuotaCount)
            {
                SetModalStatusMessage(ModalStatus.Error, "Seçilen aday için etkinlik kotası dolmuştur. Lütfen kontrol ediniz !");
                return RedirectToAction("etkinlik-listesi", "etkinlik");
            }

            _eventService.SaveEventEmployee(new EventEmployee() { Employee = currentEmployee, Event = currentEvent });
            SetModalStatusMessage(ModalStatus.Success, "Seçilen aday etkinliğe dahil edilmiştir.");
            return RedirectToAction("etkinlik-listesi", "etkinlik");
        }

        [Route("etkinlik-aday-silme")]
        public ActionResult RemoveEventEmployee(int eventID, int employeeID)
        {
            EventEmployee currentEventEmployee = _eventService.GetEventEmployeeWithID(employeeID, eventID).FirstOrDefault();
            currentEventEmployee.IsActive = false;

            SetModalStatusMessage(ModalStatus.Success, "Seçilen aday etkinlik listesinden çıkarılmıştır !");
            _eventService.SaveEventEmployee(currentEventEmployee);

            return RedirectToAction("etkinlik-listesi", "etkinlik");
        }

        [Route("etkinlik-uygun-aday-listesi")]
        public ActionResult EventAvailableEmployees(Employee employeeSearchCriteria)
        {
            PageResultSet<Employee> availableEventEmployees = _employeeService.GetActiveEmployeesFromSearch(employeeSearchCriteria, new PagingFilter());
            SetPageFilters(new PagingFilter(), availableEventEmployees.Count);

            IList<EventProfessionQuota> professionQuotaList = _eventProfessionQuotaService.GetActiveEventProfessionQuotas
                (new PagingFilter() { SearchText = (Request.QueryString["eventID"]).ToString() }).PageData;

            ViewBag.professionSelectList = professionQuotaList.Select(i => new SelectListItem()
            {
                Text = i.Profession.Title,
                Value = i.Profession.ID.ToString()
            });

            return View(availableEventEmployees.PageData);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EventAvailableEmployees(Employee employeeSearchCriteria, PagingFilter pageFilter, int eventID)
        {
            PageResultSet<Employee> availableEventEmployees = _employeeService.GetActiveEmployeesFromSearch(employeeSearchCriteria, pageFilter);
            SetPageFilters(pageFilter, availableEventEmployees.Count);
            ViewBag.EventID = eventID;
            IList<EventProfessionQuota> professionQuotaList = _eventProfessionQuotaService.GetActiveEventProfessionQuotas
                ( new PagingFilter() { SearchText = eventID.ToString() } ).PageData;

            ViewBag.professionSelectList = professionQuotaList.Select(i => new SelectListItem()
            {
                Text = i.Profession.Title,
                Value = i.Profession.ID.ToString()
            });

            return View(availableEventEmployees.PageData);
        }

        [Route("etkinlik-aday-listesi")]
        public ActionResult EventWorkingEmployees(int eventID)
        {
            IList<EventEmployee> availableEventEmployees = _eventService.GetActiveEventEmployees(eventID);
            SetPageFilters(new PagingFilter(), availableEventEmployees.Count);
            return View(availableEventEmployees);
        }
    }
}