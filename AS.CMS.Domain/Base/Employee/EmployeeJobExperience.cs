using AS.CMS.Domain.Interfaces;
using System.Runtime.Serialization;

namespace AS.CMS.Domain.Base.Employee
{
    public class EmployeeJobExperience : EntityBase, ICMSEntity
    {
        public virtual string CompanyName { get; set; }
        public virtual string Title { get; set; }
        public virtual int WorkYear { get; set; }
        public virtual int WorkMonth { get; set; }
        public virtual string QuitReason { get; set; }
        public virtual decimal Salary { get; set; }
        public virtual bool ActiveJob { get; set; }
        [IgnoreDataMember]
        public virtual Employee Employee { get; set; }
    }
}
