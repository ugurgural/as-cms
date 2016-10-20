using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace AS.CMS.Data.UOW.Helpers
{
    public class SchemaConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Schema("CMS");
        }
    }
}
