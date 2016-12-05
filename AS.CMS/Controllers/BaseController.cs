using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    public class BaseController : Controller
    {
        private IModuleService _moduleService;

        public BaseController(IModuleService moduleService)
        {
            _moduleService = moduleService;
            ViewBag.CurrentModule = GetCurrentModule();
        }

        public Module GetCurrentModule()
        {
            string moduleName = System.Web.HttpContext.Current.Request.Url.Segments[1].Replace("/", "");

            if (moduleName.LastIndexOf('?') > 0)
            {
                moduleName = moduleName.Remove(moduleName.LastIndexOf('?'));
            }

            return _moduleService.GetModuleFromPermalink(moduleName);
        }

        public void SetPageFilters(PagingFilter pageFilter, int totalRowCount)
        {
            ViewBag.PageFilter = new PagingFilter()
            {
                PageSize = pageFilter.PageSize,
                PageIndex = pageFilter.PageIndex,
                PageCount = (totalRowCount - 1) / pageFilter.PageSize + 1,
                SearchText = pageFilter.SearchText,
                PlaceHolderText = pageFilter.PlaceHolderText
            };
        }

        public void SetModalStatusMessage(ModalStatus status, string message = "")
        {
            string defaultMessage = string.Empty;
            string icon = string.Empty;
            switch (status)
            {
                case ModalStatus.Success:
                    defaultMessage = "Kayıt Başarılı.";
                    icon = "fa fa-check-circle";
                    break;
                case ModalStatus.Error:
                    defaultMessage = "Kayıt Esnasında Hata Oluştu, Tekrar Deneyiniz.";
                    icon = "fa fa-times-circle";
                    break;
                case ModalStatus.Warning:
                    //message = "Duyuru Başarıyla Kaydedildi.";
                    icon = "fa fa-warning";
                    break;
                case ModalStatus.Info:
                    //message = "Duyuru Başarıyla Kaydedildi.";
                    icon = "fa fa-info-circle";
                    break;
                default:
                    break;
            }

            TempData.Add("ModalStatus", status);
            TempData.Add("ModalStatusMessage", string.IsNullOrWhiteSpace(message) ? defaultMessage : message);
            TempData.Add("ModalStatusIcon", icon);
        }
    }
}