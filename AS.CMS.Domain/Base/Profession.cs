using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base
{
    public class Profession : EntityBase, ICMSEntity
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
    }
}