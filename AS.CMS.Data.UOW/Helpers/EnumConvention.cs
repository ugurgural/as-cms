using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace AS.CMS.Data.UOW.Helpers
{
    public class EnumConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        #region IPropertyConvention Members

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }

        #endregion

        #region IPropertyConventionAcceptance Members

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum);
        }

        #endregion

    }
}
