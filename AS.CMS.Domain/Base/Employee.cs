using AS.CMS.Domain.Common;
using AS.CMS.Domain.Interfaces;
using System;

namespace AS.CMS.Domain.Base
{
    public class Employee : EntityBase, ICMSEntity
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string MailAddress { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual int Height { get; set; }
        public virtual int Weight { get; set; }
        public virtual string Picture { get; set; }
        public virtual string CVFile { get; set; }
        public virtual string Description { get; set; }
    }
}
