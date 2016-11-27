using AS.CMS.Domain.Base.Event;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EventProfessionQuotaMappings : IAutoMappingOverride<EventProfessionQuota>
    {
        public void Override(AutoMapping<EventProfessionQuota> mapping)
        {
            mapping.References(x => x.Event, "EventID").Cascade.None();
            mapping.References(x => x.Profession, "ProfessionID").Cascade.None();
        }
    }
}