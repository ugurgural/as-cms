using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AS.CMS.Controllers
{
    [RoutePrefix("etkinlik")]
    public class EventController : ApiController
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
        public ApiResult List()
        {
            PageResultSet<Event> activeEvents = _eventService.GetActiveEvents(new PagingFilter());
            return new ApiResult() { Data = activeEvents.PageData, Message = "OK", Success = true };
        }

        [Route("etkinlik/{eventID}")]
        [HttpGet]
        public ApiResult Get(int eventID)
        {
            return new ApiResult() { Data = _eventService.GetEventWithID(eventID), Message = "OK", Success = true };
        }

        [HttpPost]
        [Route("etkinlik-kayit")]
        public ApiResult SaveEvent(Event eventEntity, int eventTypeID, string[] eventDaysList)
        {
            eventEntity.EventType = new EventType() { ID = eventTypeID };

            if (eventDaysList.Length > 0)
            {
                eventEntity.EventDays = string.Join(",", eventDaysList);
            }

            bool result = _eventService.SaveEvent(eventEntity);

            return new ApiResult() { Data = null, Message = "OK", Success = result };
        }

        [HttpPost]
        [Route("etkinlik-aday-kayit")]
        public ApiResult SaveEventEmployee(int eventID, int employeeID, int professionID)
        {
            Employee currentEmployee = _employeeService.GetEmployeeWithID(employeeID);
            Event currentEvent = _eventService.GetEventWithID(eventID);

            int eventProfessionQuotaCount = _eventProfessionQuotaService.GetEventProfessionQuotaWithProfessionID(eventID, professionID).Where(x => x.Gender == currentEmployee.Gender).Count();

            int employeeProfessionCountForCurrentEvent = _eventService.GetActiveEventEmployees(eventID)
                                                         .Where(x => x.Employee.Profession.Where(y => y.ID == professionID).Any() && x.Employee.Gender == currentEmployee.Gender).Count();

            var employeeActiveEvents = _eventService.GetActiveEmployeeEvents(employeeID);

            if (string.IsNullOrWhiteSpace(currentEvent.EventDays))
            {
                return new ApiResult() { Data = null, Message = "Etkinlik için herhangi bir çalışma günü belirlenmemiş. Lütfen kontrol ediniz !", Success = false };
            }

            if (!string.IsNullOrWhiteSpace(currentEmployee.EmployeeAvailability[0].WorkDays))
            {
                string[] employeeWorkDays = currentEmployee.EmployeeAvailability[0].WorkDays.Split(',');
                string[] eventWorkDays = currentEvent.EventDays.Split(',');

                for (int i = 0; i < eventWorkDays.Length; i++)
                {
                    if (!employeeWorkDays.Contains(eventWorkDays[i]))
                    {
                        return new ApiResult() { Data = null, Message = "Seçilen adayın çalışma günleri etkinlik günü ile örtüşmemektedir. Lütfen kontrol ediniz !", Success = false };
                    }
                }
            }
            else
            {
                return new ApiResult() { Data = null, Message = "Seçilen aday uygun olduğu herhangi bir çalışma günü belirlememiş. Lütfen kontrol ediniz !", Success = false };
            }

            if (employeeActiveEvents != null && employeeActiveEvents.Count > 0)
            {
                foreach (var employeeEventItem in employeeActiveEvents)
                {
                    if (employeeEventItem.Event.ID.Equals(currentEvent.ID))
                    {
                        return new ApiResult() { Data = null, Message = "Seçilen aday bu etkinliğe zaten eklenmiş. Lütfen kontrol ediniz !", Success = false };
                    }

                    string[] employeeEventWorkDays = employeeEventItem.Event.EventDays.Split(',');
                    string[] currentEventWorkDays = currentEvent.EventDays.Split(',');

                    for (int i = 0; i < employeeEventWorkDays.Length; i++)
                    {
                        if (currentEventWorkDays.Contains(employeeEventWorkDays[i]))
                        {
                            return new ApiResult() { Data = null, Message = "Seçilen adayın etkinliğin olduğu günlerde katıldığı başka bir etkinlik bulunmakta. Lütfen kontrol ediniz !", Success = false };
                        }
                    }
                }
            }

            if (employeeProfessionCountForCurrentEvent >= eventProfessionQuotaCount)
            {
                return new ApiResult() { Data = null, Message = "Seçilen aday için etkinlik kotası dolmuştur. Lütfen kontrol ediniz !", Success = false };
            }

            _eventService.SaveEventEmployee(new EventEmployee() { Employee = currentEmployee, Event = currentEvent });

            return new ApiResult() { Data = null, Message = "Seçilen aday etkinliğe dahil edilmiştir.", Success = true };
        }

        [Route("etkinlik-aday-sayi/{eventID}")]
        [HttpGet]
        public ApiResult EventWorkingEmployees(int eventID)
        {
            IList<EventEmployee> availableEventEmployees = _eventService.GetActiveEventEmployees(eventID);
            return new ApiResult() { Data = availableEventEmployees.Count, Message = "OK", Success = true };
        }
    }
}