using AS.CMS.Domain.Common;
using AS.CMS.Domain.Interfaces;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace AS.CMS.Data.Interfaces
{
    public interface IBaseRepository<T> where T : ICMSEntity
    {
        IList<T> GetAll();
        T GetById(int id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(int id);
        IList<T> GetWithCriteria(DetachedCriteria criteria);
        IList<T> GetWithCriteria(DetachedCriteria criteria, Order order);
        int GetRowCountWithCriteria(DetachedCriteria criteria);
        IList<T> GetWithCriteriaByPaging(DetachedCriteria criteria, PagingFilter pagingFilter);
        IList<T> GetWithCriteriaByPaging(DetachedCriteria criteria, PagingFilter pagingFilter, Order order);
        IList<T> GetParent(int parentID);
    }
}
