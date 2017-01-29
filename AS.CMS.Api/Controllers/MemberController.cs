using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AS.CMS.Controllers
{
    [RoutePrefix("uye")]
    [CustomAuthorize(Roles = "Admin, Editor")]
    public class MemberController : BaseController
    {
        private IMemberService _memberService;
        private IRoleService _roleService;

        public MemberController(IMemberService memberService, IRoleService roleService, IModuleService moduleService) : base(moduleService)
        {
            _memberService = memberService;
            _roleService = roleService;
        }

        [Route("uye-listesi")]
        public ActionResult Index(PagingFilter pageFilter)
        {
            PageResultSet<Member> activeMembers = _memberService.GetActiveMembers(pageFilter);
            pageFilter.PlaceHolderText = "Mail adreslerinde ara";
            SetPageFilters(pageFilter, activeMembers.Count);

            return View(activeMembers.PageData);
        }

        [Route("yeni-uye-ekle")]
        public ActionResult AddOrEdit(int? memberID)
        {
            Member currentMember = new Member();

            if (memberID.HasValue && memberID.Value > 0)
            {
                currentMember = _memberService.GetMemberWithID(memberID.Value);
            }

            return View(currentMember);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveMember(Member memberEntity, string memberPicture)
        {
            memberEntity.Picture = memberPicture;

            if (!string.IsNullOrWhiteSpace(memberEntity.Password))
            {
                memberEntity.Password = UtilityHelper.GenerateMD5Hash(memberEntity.Password);
            }

            memberEntity.Roles.Add(_roleService.GetRoleWithID(1));
            memberEntity.Roles.Add(_roleService.GetRoleWithID(2));
            memberEntity.Roles.Add(_roleService.GetRoleWithID(3));
            bool result = _memberService.SaveMember(memberEntity);

            if (result)
            {
                SetModalStatusMessage(ModalStatus.Success);
            }
            else
            {
                SetModalStatusMessage(ModalStatus.Error);
            }

            return RedirectToAction("uye-listesi", "uye");
        }

    }
}