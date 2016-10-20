using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IAnnouncementService
    {
        bool SaveAnnouncement(Announcement announcementEntity);
        PageResultSet<Announcement> GetActiveAnnouncements(PagingFilter pagingFilter);
        Announcement GetAnnouncementWithID(int announcementID);
    }
}
