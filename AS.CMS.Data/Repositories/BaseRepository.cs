using AS.CMS.Data.UOW.Helpers;
using AS.CMS.Data.Interfaces;
using AS.CMS.Data.UOW.Interfaces;
using AS.CMS.Domain.Interfaces;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Criterion;
using AS.CMS.Domain.Common;
using System;

namespace AS.CMS.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : ICMSEntity
    {
        private UnitOfWork _unitOfWork;
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        protected ISession Session { get { return _unitOfWork.Session; } }

        public IList<T> GetAll()
        {
            IList<T> result;
            _unitOfWork.BeginTransaction();
            result = _unitOfWork.Session.CreateCriteria(typeof(T)).List<T>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();

            return result;
        }

        public IList<T> GetWithCriteria(DetachedCriteria criteria)
        {
            IList<T> result;

            _unitOfWork.BeginTransaction();
            result = criteria.GetExecutableCriteria(_unitOfWork.Session).List<T>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();


            return result;
        }

        public IList<T> GetWithCriteria(DetachedCriteria criteria, Order order)
        {
            IList<T> result;

            _unitOfWork.BeginTransaction();
            result = criteria.GetExecutableCriteria(_unitOfWork.Session).AddOrder(order).List<T>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();


            return result;
        }

        public int GetRowCountWithCriteria(DetachedCriteria criteria)
        {
            int result = 0;

            _unitOfWork.BeginTransaction();
            result = criteria.GetExecutableCriteria(_unitOfWork.Session)
                         .SetProjection(Projections.RowCount()).UniqueResult<int>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();


            return result;
        }

        public IList<T> GetWithCriteriaByPaging(DetachedCriteria criteria, PagingFilter pagingFilter)
        {
            IList<T> result;

            _unitOfWork.BeginTransaction();
            result = criteria
                    .SetFirstResult((pagingFilter.PageIndex - 1) * pagingFilter.PageSize)
                    .SetMaxResults(pagingFilter.PageSize)
                    .GetExecutableCriteria(_unitOfWork.Session).List<T>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();

            return result;
        }

        public IList<T> GetWithCriteriaByPaging(DetachedCriteria criteria, PagingFilter pagingFilter, Order order)
        {
            IList<T> result;

            _unitOfWork.BeginTransaction();
            result = criteria
                    .SetFirstResult((pagingFilter.PageIndex - 1) * pagingFilter.PageSize)
                    .SetMaxResults(pagingFilter.PageSize)
                    .GetExecutableCriteria(_unitOfWork.Session)
                    .AddOrder(order).List<T>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();

            return result;
        }

        public IList<T> GetParent(int parentID)
        {
            IList<T> result;

            _unitOfWork.BeginTransaction();
            result = _unitOfWork.Session.CreateCriteria(typeof(T))
                         .Add(Restrictions.Eq("ParentID", parentID))
                         .List<T>();
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();

            return result;
        }

        public T GetById(int id)
        {
            T result;
            _unitOfWork.BeginTransaction();
            result = _unitOfWork.Session.Get<T>(id);
            _unitOfWork.Session.Flush();
            _unitOfWork.Commit();


            return result;
        }

        public bool Create(T entity)
        {
            bool result = false;

            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Session.Save(entity);
                _unitOfWork.Session.Flush();
                _unitOfWork.Commit();
                result = true;
            }
            catch ( Exception ex)
            { }
            

            return result;
        }

        public bool Update(T entity)
        {
            bool result = false;

            try
            {
                
                _unitOfWork.BeginTransaction();
                _unitOfWork.Session.Clear();
                _unitOfWork.Session.Update(entity);
                _unitOfWork.Session.Flush();
                _unitOfWork.Commit();
                result = true;
            }
            catch (Exception ex)
            { }

            return result;
        }

        public bool Delete(int id)
        {
            bool result = false;

            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Session.Delete(Session.Load<T>(id));
                _unitOfWork.Session.Flush();
                _unitOfWork.Commit();
                result = true;
            }
            catch { }

            return result;
        }
    }
}