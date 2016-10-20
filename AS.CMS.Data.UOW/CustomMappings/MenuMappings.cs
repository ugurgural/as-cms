using AS.CMS.Domain.Base;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;


namespace AS.CMS.Data.UOW.CustomMappings
{
    public class MenuMappings : IAutoMappingOverride<Menu>
    {
        public void Override(AutoMapping<Menu> mapping)
        {
            //TODO: Override normal mapping işlemleri dışında ekleme yaparken kullanılır.
            //mapping.Map(x => x.CreatedDate, "CreatedDate").Access.Property().Not.Nullable().Default("getdate()");
            //mapping.Map(x => x.IsActive, "IsActive").Access.Property().Not.Nullable().Default("true");
        }
    }
}
