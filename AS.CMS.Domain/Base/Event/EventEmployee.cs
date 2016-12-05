using AS.CMS.Domain.Interfaces;
using System.Collections.Generic;

namespace AS.CMS.Domain.Base.Event
{
    public class EventEmployee : EntityBase, ICMSEntity
    {
        public virtual Employee.Employee Employee { get; set; }
        public virtual Event Event { get; set; }
    }
}