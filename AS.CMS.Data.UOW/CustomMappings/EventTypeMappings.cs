using AS.CMS.Domain.Base.Event;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EventTypeMappings : IAutoMappingOverride<EventType>
    {
        public void Override(AutoMapping<EventType> mapping)
        {
            mapping.HasMany(x => x.Events).KeyColumn("EventTypeID").Inverse().Cascade.All();           
        }
    }
}
