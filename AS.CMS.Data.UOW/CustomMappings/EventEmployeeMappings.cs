using AS.CMS.Domain.Base.Event;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using System.Linq;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EventEmployeeMappings : IAutoMappingOverride<EventEmployee>
    {
        public void Override(AutoMapping<EventEmployee> mapping)
        {
            mapping.References(x => x.Employee, "EmployeeID").Cascade.None();
            mapping.References(x => x.Event, "EventID").Cascade.None();
        }
    }
}
