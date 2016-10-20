using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;

namespace AS.CMS.Business.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        #region Repository Injection

        private IBaseRepository<Announcement> _announcementRepository;

        public AnnouncementService(IBaseRepository<Announcement> announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveAnnouncement(Announcement announcementEntity)
        {
            bool result = false;

            if (announcementEntity.ID == 0)
            {
                result = _announcementRepository.Create(announcementEntity);
            }
            else
            {
                result = _announcementRepository.Update(announcementEntity);
            }

            return result;
        }

        public PageResultSet<Announcement> GetActiveAnnouncements(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Announcement>("Title", pagingFilter.SearchText);
            DetachedCriteria activeAnnouncementPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Announcement>("Title", pagingFilter.SearchText);

            return new PageResultSet<Announcement>()
            {
                PageData = _announcementRepository.GetWithCriteriaByPaging(activeAnnouncementPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _announcementRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Announcement GetAnnouncementWithID(int announcementID)
        {
            return _announcementRepository.GetById(announcementID);
        }

        #endregion
    }
}