using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("meslek")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class ProfessionController : BaseController
    {
        private IProfessionService _professionService;

        public ProfessionController(IProfessionService professionService, IModuleService moduleService) : base(moduleService)
        {
            _professionService = professionService;
        }

        [Route("meslek-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<Profession> activeProfessions = _professionService.GetActiveProfessions(pageFilter);
            pageFilter.PlaceHolderText = "İsimlerde ara";
            SetPageFilters(pageFilter, activeProfessions.Count);

            return View(activeProfessions.PageData);
        }

        [Route("yeni-meslek-ekle")]
        public ActionResult AddOrEdit(int? professionID)
        {
            Profession currentProfession = new Profession();

            if (professionID.HasValue && professionID.Value > 0)
            {
                currentProfession = _professionService.GetProfessionWithID(professionID.Value);
            }

            return View(currentProfession);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveProfession(Profession professionEntity)
        {
            bool result = _professionService.SaveProfession(professionEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("meslek-listesi", "meslek");
        }
    }
}