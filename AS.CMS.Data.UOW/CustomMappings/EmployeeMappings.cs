using AS.CMS.Domain.Base.Employee;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class EmployeeMappings : IAutoMappingOverride<Employee>
    {
        public void Override(AutoMapping<Employee> mapping)
        {

            mapping.HasMany(x => x.EmployeeAvailability).Inverse().Cascade.All();
            mapping.HasMany(x => x.EmployeeCertificateAndLanguage).Inverse().Cascade.All();
            mapping.HasMany(x => x.EmployeeEducation).Inverse().Cascade.All();
            mapping.HasMany(x => x.EmployeeJobExperience).Inverse().Cascade.All();

            mapping.HasManyToMany(x => x.Profession).Cascade.All()
            .Table("CMS.EmployeeProfession")
            .ParentKeyColumn("EmployeeID")
            .ChildKeyColumn("ProfessionID");
        }
    }
}