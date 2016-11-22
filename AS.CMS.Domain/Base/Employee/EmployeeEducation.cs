using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base.Employee
{
    public class EmployeeEducation : EntityBase, ICMSEntity
    {
        public virtual string OrganizationName { get; set; }
        public virtual string Institute { get; set; }
        public virtual string Degree { get; set; }
        public virtual bool ActiveEducation { get; set; }
        public virtual Employee Employee { get; set; }
    }
}