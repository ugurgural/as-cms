using AS.CMS.Domain.Base.Employee;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EmployeeCertificateAndLanguageMappings : IAutoMappingOverride<EmployeeCertificateAndLanguage>
    {
        public void Override(AutoMapping<EmployeeCertificateAndLanguage> mapping)
        {
            mapping.References<Employee>(x => x.Employee).Column("EmployeeID").Not.Nullable();
        }
    }
}