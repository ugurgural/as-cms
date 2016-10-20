using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class ContentService : IContentService
    {
        #region Repository Injection

        private IBaseRepository<Content> _contentRepository;

        public ContentService(IBaseRepository<Content> contentRepository)
        {
            _contentRepository = contentRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveContent(Content contentEntity)
        {
            if (contentEntity.ID == 0)
            {
                _contentRepository.Create(contentEntity);
            }
            else
            {
                _contentRepository.Update(contentEntity);
            }

            return true;
        }

        public PageResultSet<Content> GetActiveContents(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Content>("Title", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Content>("Title", pagingFilter.SearchText);

            return new PageResultSet<Content>()
            {
                PageData = _contentRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _contentRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Content GetContentWithID(int contentID)
        {
            return _contentRepository.GetById(contentID);
        }

        #endregion
    }
}