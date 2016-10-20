using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("menu")]
    [CustomAuthorize(Roles = "Admin")]
    public class MenuController : BaseController
    {
        private IMenuService _menuService;

        public MenuController(IMenuService menuService, IModuleService moduleService) : base(moduleService)
        {
            _menuService = menuService;
        }

        [Route("yeni-menu-ekle")]
        public ActionResult AddOrEdit(int? menuID)
        {
            Menu currentMenu = new Menu();

            if (menuID.HasValue && menuID.Value > 0)
            {
                currentMenu = _menuService.GetMenuWithID(menuID.Value);
            }

            ViewBag.Menu = _menuService.GetPublishedMenuItems();

            return View(currentMenu);
        }

        [Route("menu-listesi")]
        public ActionResult Index()
        {
            ViewBag.Menu = _menuService.GetPublishedMenuItems();
            return View();
        }

        [HttpPost]
        public ActionResult SaveMenu(Menu menuEntity)
        {
            bool result = _menuService.SaveMenuItem(menuEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveMenuPosition(string hdnMenuOrder)
        {
            bool result = _menuService.SaveMenuPosition(hdnMenuOrder);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult RemoveMenuItem(int menuItemID)
        {
            _menuService.RemoveMenuItem(menuItemID);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}