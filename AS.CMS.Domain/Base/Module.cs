using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base
{
    public class Module : EntityBase, ICMSEntity
    {
        public virtual string Name { get; set; }
        public virtual string FriendlyName { get; set; }
        public virtual string Description { get; set; }
    }
}
