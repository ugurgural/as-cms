using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base
{
    public class Menu : EntityBase, ICMSEntity
    {
        public virtual int ParentID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Caption { get; set; }
        public virtual int ItemType { get; set; }
        public virtual string Permalink { get; set; }
        public virtual string PermalinkTarget { get; set; }
        public virtual int ItemOrder { get; set; }
        public virtual bool IsPublished { get; set; }
    }
}
