using AS.CMS.Domain.Common;
using System.Web;

namespace AS.CMS.Business.Helpers
{
    public static class MemberHelper
    {
        public static CustomPrincipal GetCurrentMember()
        {
            return HttpContext.Current.User as CustomPrincipal;
        }
    }
}
