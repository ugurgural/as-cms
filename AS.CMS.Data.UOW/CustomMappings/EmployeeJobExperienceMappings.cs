using AS.CMS.Domain.Base.Employee;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EmployeeJobExperienceMappings : IAutoMappingOverride<EmployeeJobExperience>
    {
        public void Override(AutoMapping<EmployeeJobExperience> mapping)
        {
            mapping.References<Employee>(x => x.Employee).Column("EmployeeID").Not.Nullable();
        }
    }
}