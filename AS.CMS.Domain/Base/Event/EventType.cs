using AS.CMS.Domain.Interfaces;
using System.Collections.Generic;

namespace AS.CMS.Domain.Base.Event
{
    public class EventType : EntityBase, ICMSEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<Event> Events { get; protected set; }
    }
}
