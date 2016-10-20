 using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base
{
    public class Role : EntityBase, ICMSEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
