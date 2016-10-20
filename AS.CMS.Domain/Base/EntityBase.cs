using System;

namespace AS.CMS.Domain.Base
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }

        public virtual int ID { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual int CreatedBy { get; set; }
    }
}
