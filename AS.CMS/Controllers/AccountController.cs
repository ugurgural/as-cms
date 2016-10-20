using AS.CMS.Domain.Common;
using AS.CMS.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;

namespace AS.CMS.Controllers
{
    [RoutePrefix("hesap")]
    public class AccountController : Controller
    {
        private IMemberService _memberService;

        public AccountController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [AllowAnonymous]
        [Route("giris-yap")]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ValidateUser(string userName, string password)
        {
            if (ModelState.IsValid)
            {

                var member = _memberService.GetMember(userName, password);
                if (member != null)
                {
                    var roles = member.Roles.Select(r => r.Name).ToArray();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = member.ID;
                    serializeModel.FirstName = member.FirstName;
                    serializeModel.LastName = member.LastName;
                    serializeModel.Picture = member.Picture;
                    serializeModel.Roles = roles;

                    string memberData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             member.Email,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(15),
                             false,
                             memberData);

                    string encryptionTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptionTicket);
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Home");

                }

                ViewBag.ErrorMessage = "Hatalı Kullanıcı Adı Veya Şifre Girdiniz.";
            }

            return View("Login");
        }

        [AllowAnonymous]
        [Route("cikis-yap")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
    }
}