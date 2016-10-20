using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;

namespace AS.CMS.Business.Interfaces
{
    public interface IMemberService
    {
        PageResultSet<Member> GetActiveMembers(PagingFilter pagingFilter);
        Member GetMember(string userName, string password);
        Member GetMemberWithID(int memberID);
        bool SaveMember(Member memberEntity);
    }
}
