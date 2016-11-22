using AS.CMS.Domain.Interfaces;

namespace AS.CMS.Domain.Base.Employee
{
    public class EmployeeAvailability : EntityBase, ICMSEntity
    {
        public virtual string WorkType { get; set; }
        public virtual string WorkDays { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
