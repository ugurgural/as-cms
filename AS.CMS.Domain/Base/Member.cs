using AS.CMS.Domain.Interfaces;
using System.Collections.Generic;

namespace AS.CMS.Domain.Base
{
    public class Member : EntityBase, ICMSEntity
    {
        public Member()
        {
            this.Roles = new List<Role>();
        }

        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Picture { get; set; }
        public virtual ICollection<Role> Roles { get; protected set; }
    }
}
