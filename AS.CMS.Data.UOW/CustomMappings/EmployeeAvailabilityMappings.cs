using AS.CMS.Domain.Base.Employee;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EmployeeAvailabilityMappings : IAutoMappingOverride<EmployeeAvailability>
    {
        public void Override(AutoMapping<EmployeeAvailability> mapping)
        {
            mapping.References<Employee>(x => x.Employee).Column("EmployeeID").Not.Nullable();
        }
    }
}