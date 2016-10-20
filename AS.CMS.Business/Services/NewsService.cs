using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class NewsService : INewsService
    {
        #region Repository Injection

        private IBaseRepository<News> _newsRepository;

        public NewsService(IBaseRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveNews(News newsEntity)
        {
            if (newsEntity.ID == 0)
            {
                _newsRepository.Create(newsEntity);
            }
            else
            {
                _newsRepository.Update(newsEntity);
            }

            return true;
        }

        public PageResultSet<News> GetActiveNews(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<News>("Title", pagingFilter.SearchText);
            DetachedCriteria activeNewsPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<News>("Title", pagingFilter.SearchText);

            return new PageResultSet<News>()
            {
                PageData = _newsRepository.GetWithCriteriaByPaging(activeNewsPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _newsRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public News GetNewsWithID(int newsID)
        {
            return _newsRepository.GetById(newsID);
        }

        #endregion
    }
}