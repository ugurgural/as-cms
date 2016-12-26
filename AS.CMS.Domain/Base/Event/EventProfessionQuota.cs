using AS.CMS.Domain.Common;
using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base.Event
{
    public class EventProfessionQuota : EntityBase, ICMSEntity
    {
        public virtual Event Event { get; set; }
        public virtual Profession Profession { get; set; }
        public virtual int Quantity { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual double Price { get; set; }
    }
}