using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Web.Http;

namespace AS.CMS.Controllers
{
    [RoutePrefix("api/meslek")]
    public class ProfessionController : ApiController
    {
        private IProfessionService _professionService;

        public ProfessionController(IProfessionService professionService)
        {
            _professionService = professionService;
        }

        [Route("meslek-listesi")]
        [HttpGet]
        public ApiResult List()
        {
            PageResultSet<Profession> activeProfessions = _professionService.GetActiveProfessions(new PagingFilter());
            return new ApiResult() { Data = activeProfessions.PageData, Message = "OK", Success = true };
        }

        [Route("meslek/{professionID}")]
        public ApiResult Get(int professionID)
        {
            return new ApiResult() { Data = _professionService.GetProfessionWithID(professionID), Message = "OK", Success = true };
        }

        [HttpPost]
        [Route("meslek-kayit")]
        public ApiResult SaveProfession(Profession professionEntity)
        {
            bool result = _professionService.SaveProfession(professionEntity);
            return new ApiResult() { Data = null, Message = "OK", Success = result };
        }
    }
}