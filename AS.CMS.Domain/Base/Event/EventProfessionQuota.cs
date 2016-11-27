using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base.Event
{
    public class EventProfessionQuota : EntityBase, ICMSEntity
    {
        public Event Event { get; set; }
        public Profession Profession { get; set; }
        public int Quantity { get; set; }
    }
}