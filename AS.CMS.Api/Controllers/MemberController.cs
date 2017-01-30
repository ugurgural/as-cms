using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using AS.CMS.Domain.Dto;
using System.Web.Http;

namespace AS.CMS.Controllers
{
    [RoutePrefix("uye")]
    public class MemberController : ApiController
    {
        private IMemberService _memberService;
        private IRoleService _roleService;

        public MemberController(IMemberService memberService, IRoleService roleService)
        {
            _memberService = memberService;
            _roleService = roleService;
        }

        [Route("uye-listesi")]
        public ApiResult List(PagingFilter pageFilter)
        {
            PageResultSet<Member> activeMembers = _memberService.GetActiveMembers(pageFilter);

            return new ApiResult() { Data = activeMembers.PageData, Message = "OK", Success = true };
        }

        [Route("uye/{memberID}")]
        public ApiResult Get(int memberID)
        {
            return new ApiResult() { Data = _memberService.GetMemberWithID(memberID), Message = "OK", Success = true };
        }

        [HttpPost]
        [Route("uye-kayit")]
        public ApiResult SaveMember(Member memberEntity, string memberPicture)
        {
            memberEntity.Picture = memberPicture;

            if (!string.IsNullOrWhiteSpace(memberEntity.Password))
            {
                memberEntity.Password = UtilityHelper.GenerateMD5Hash(memberEntity.Password);
            }

            memberEntity.Roles.Add(_roleService.GetRoleWithID(4));
            bool result = _memberService.SaveMember(memberEntity);

            return new ApiResult() { Data = null, Message = "OK", Success = result };
        }
    }
}