using AS.CMS.Domain.Common;
using AS.CMS.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;

namespace AS.CMS.Api.Controllers
{
    [RoutePrefix("hesap")]
    public class AccountController : Controller
    {
        private IMemberService _memberService;

        public AccountController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost]
        [Route("giris-yap")]
        public CustomPrincipalSerializeModel Login(string userName, string password)
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

                return memberModel;
            }
            else
            {
                return null;
            }
        }
    }
}