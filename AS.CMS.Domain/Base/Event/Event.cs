using AS.CMS.Domain.Interfaces;
using System;

namespace AS.CMS.Domain.Base.Event
{
    public class Event : EntityBase, ICMSEntity
    {
        public Event()
        {
            this.EventType = new EventType();
        }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual double Price { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string EventDays { get; set; }
        public virtual string Location { get; set; }
        public virtual string Restriction { get; set; }
        public virtual string EventDocument { get; set; }
    }
}
