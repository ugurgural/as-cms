using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("duyuru")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class AnnouncementController : BaseController
    {
        private IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService, IModuleService moduleService) : base(moduleService)
        {
            _announcementService = announcementService;
        }

        [Route("duyuru-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<Announcement> activeAnnouncements = _announcementService.GetActiveAnnouncements(pageFilter);
            pageFilter.PlaceHolderText = "Başlıklarda ara";
            SetPageFilters(pageFilter, activeAnnouncements.Count);

            return View(activeAnnouncements.PageData);
        }

        [Route("yeni-duyuru-ekle")]
        public ActionResult AddOrEdit(int? announcementID)
        {
            Announcement currentNews = new Announcement();

            if (announcementID.HasValue && announcementID.Value > 0)
            {
                currentNews = _announcementService.GetAnnouncementWithID(announcementID.Value);
            }

            return View(currentNews);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveAnnouncement(Announcement announcementEntity, string pageImage, string galleryImages)
        {
            announcementEntity.EditorImageList = pageImage;
            announcementEntity.ImageGallery = galleryImages;
            bool result = _announcementService.SaveAnnouncement(announcementEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("duyuru-listesi", "duyuru");
        }
    }
}