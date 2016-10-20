using AS.CMS.Domain.Base;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AS.CMS.Data.UOW.CustomMappings
{
    public class MemberMappings : IAutoMappingOverride<Member>
    {
        public void Override(AutoMapping<Member> mapping)
        {
            mapping.HasManyToMany(x => x.Roles).Cascade.All()
            .Table("CMS.MemberRole")
            .ParentKeyColumn("MemberID")
            .ChildKeyColumn("RoleID");
        }
    }
}