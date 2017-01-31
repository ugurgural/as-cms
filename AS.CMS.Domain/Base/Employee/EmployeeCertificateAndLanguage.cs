using AS.CMS.Domain.Interfaces;
using System;
using System.Runtime.Serialization;

namespace AS.CMS.Domain.Base.Employee
{
    public class EmployeeCertificateAndLanguage : EntityBase, ICMSEntity
    {
        public EmployeeCertificateAndLanguage()
        {
            this.CompletedDate = DateTime.Now;
        }

        public virtual string Title { get; set; }
        public virtual string OrganizationName { get; set; }
        public virtual int ReadingRate { get; set; }
        public virtual int WritingRate { get; set; }
        public virtual int SpeakingRate { get; set; }
        public virtual DateTime CompletedDate { get; set; }
        [IgnoreDataMember]
        public virtual Employee Employee { get; set; }
    }
}
