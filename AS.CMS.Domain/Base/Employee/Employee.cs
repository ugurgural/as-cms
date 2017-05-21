using AS.CMS.Domain.Common;
using AS.CMS.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AS.CMS.Domain.Base.Employee
{
    public class Employee : EntityBase, ICMSEntity
    {
        public Employee()
        {
            EmployeeAvailability = new List<EmployeeAvailability>();
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string TCIdentityNo { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string MailAddress { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string BirthPlace { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual int Height { get; set; }
        public virtual int Weight { get; set; }
        public virtual string Picture { get; set; }
        public virtual string CVFile { get; set; }
        public virtual string Description { get; set; }
        public virtual string HairColor { get; set; }
        public virtual string EyeColor { get; set; }
        public virtual bool HasDriverLicense { get; set; }
        public virtual bool ActiveCarDriver { get; set; }
        public virtual string UpperBodySize { get; set; }
        public virtual string LowerBodySize { get; set; }
        public virtual string ChestSize { get; set; }
        public virtual string WaistSize { get; set; }
        public virtual string HipSize { get; set; }
        public virtual string ShoeSize { get; set; }
        public virtual string JacketSize { get; set; }
        public virtual string PantSize { get; set; }
        public virtual string JeanSize { get; set; }
        public virtual string SkirtSize { get; set; }
        public virtual EmployeeStatus Status { get; set; }
        public virtual bool IsFacebookUser { get; set; }
        public virtual IList<EmployeeAvailability> EmployeeAvailability { get; set; }
        public virtual IList<EmployeeCertificateAndLanguage> EmployeeCertificateAndLanguage { get; set; }
        public virtual IList<EmployeeEducation> EmployeeEducation { get; set; }
        public virtual IList<EmployeeJobExperience> EmployeeJobExperience { get; set; }
        public virtual IList<Profession> Profession { get; set; }
    }
}
