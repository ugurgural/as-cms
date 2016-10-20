using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("icerik")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class ContentController : BaseController
    {
        private IContentService _contentService;

        public ContentController(IContentService contentService, IModuleService moduleService) : base(moduleService)
        {
            _contentService = contentService;
        }

        [Route("icerik-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<Content> activeContents = _contentService.GetActiveContents(pageFilter);
            pageFilter.PlaceHolderText = "Başlıklarda ara";
            SetPageFilters(pageFilter, activeContents.Count);

            return View(activeContents.PageData);
        }

        [Route("yeni-icerik-ekle")]
        public ActionResult AddOrEdit(int? contentID)
        {
            Content currentContent = new Content();

            if (contentID.HasValue && contentID.Value > 0)
            {
                currentContent = _contentService.GetContentWithID(contentID.Value);
            }

            return View(currentContent);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveContent(Content contentEntity, string pageImage, string galleryImages)
        {
            contentEntity.EditorImageList = pageImage;
            contentEntity.ImageGallery = galleryImages;
            bool result = _contentService.SaveContent(contentEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("icerik-listesi", "icerik");
        }
    }
}