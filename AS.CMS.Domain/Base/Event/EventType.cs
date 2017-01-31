using AS.CMS.Domain.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AS.CMS.Domain.Base.Event
{
    public class EventType : EntityBase, ICMSEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<Event> Events { get; protected set; }
    }
}
