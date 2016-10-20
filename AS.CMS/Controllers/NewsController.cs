using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("haber")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class NewsController : BaseController
    {
        private INewsService _newsService;

        public NewsController(INewsService newsService, IModuleService moduleService) : base(moduleService)
        {
            _newsService = newsService;
        }

        [Route("haber-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<News> activeNews = _newsService.GetActiveNews(pageFilter);
            pageFilter.PlaceHolderText = "Başlıklarda ara";
            SetPageFilters(pageFilter, activeNews.Count);

            return View(activeNews.PageData);
        }

        [Route("yeni-haber-ekle")]
        public ActionResult AddOrEdit(int? newsID)
        {
            News currentNews = new News();

            if (newsID.HasValue && newsID.Value > 0)
            {
                currentNews = _newsService.GetNewsWithID(newsID.Value);
            }

            return View(currentNews);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveNews(News newsEntity, string pageImage, string galleryImages)
        {
            newsEntity.EditorImageList = pageImage;
            newsEntity.ImageGallery = galleryImages;
            bool result = _newsService.SaveNews(newsEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("haber-listesi", "haber");
        }
    }
}