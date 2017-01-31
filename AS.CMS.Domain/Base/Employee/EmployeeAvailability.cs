using AS.CMS.Domain.Interfaces;
using System.Runtime.Serialization;

namespace AS.CMS.Domain.Base.Employee
{
    public class EmployeeAvailability : EntityBase, ICMSEntity
    {
        public virtual string WorkType { get; set; }
        public virtual string WorkDays { get; set; }
        [IgnoreDataMember]
        public virtual Employee Employee { get; set; }
    }
}
