using AS.CMS.Domain.Base.Event;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EventMappings : IAutoMappingOverride<Event>
    {
        public void Override(AutoMapping<Event> mapping)
        {
            mapping.References(x => x.EventType, "EventTypeID").Cascade.None();
        }
    }
}