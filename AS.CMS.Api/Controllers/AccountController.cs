using AS.CMS.Domain.Common;
using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Dto;
using System.Linq;
using System.Web.Http;

namespace AS.CMS.Api.Controllers
{
    [RoutePrefix("api/hesap")]
    public class AccountController : ApiController
    {
        private IMemberService _memberService;

        public AccountController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost]
        [Route("giris-yap")]
        public ApiResult Login(string userName, string password)
        {
            var member = _memberService.GetMember(userName, password);
            if (member != null)
            {
                var roles = member.Roles.Select(r => r.Name).ToArray();

                CustomPrincipalSerializeModel memberModel = new CustomPrincipalSerializeModel();
                memberModel.UserId = member.ID;
                memberModel.FirstName = member.FirstName;
                memberModel.LastName = member.LastName;
                memberModel.Picture = member.Picture;
                memberModel.Roles = roles;

                return new ApiResult() { Data = memberModel, Message = "OK", Success = true };
            }
            else
            {
                return new ApiResult() { Data = null, Message = "Hatalı Giriş", Success = false };
            }
        }
    }
}