using AS.CMS.Domain.Base.Employee;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EmployeeEducationMappings : IAutoMappingOverride<EmployeeEducation>
    {
        public void Override(AutoMapping<EmployeeEducation> mapping)
        {
            mapping.References<Employee>(x => x.Employee).Column("EmployeeID").Not.Nullable();
        }
    }
}