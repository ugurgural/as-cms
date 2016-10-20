using NHibernate.Criterion;

namespace AS.CMS.Business.Helpers
{
    public static class CriteriaHelper
    {
        public static DetachedCriteria GetDefaultCriteria<T>()
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<T>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return defaultCriteria;
        }

        public static DetachedCriteria GetDefaultSearchCriteria<T>(string searchField = "", string searchText = "")
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<T>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchText))
            {
                defaultCriteria.Add(Expression.Like(searchField, searchText, MatchMode.Anywhere));
                defaultCriteria.Add(Expression.Like(searchField, searchText, MatchMode.Anywhere));
            }

            return defaultCriteria;
        }

        public static Order GetDefaultOrder(string propertyName = "ID", bool ascending = true)
        {
            return new Order(propertyName, ascending);
        }
    }
}
