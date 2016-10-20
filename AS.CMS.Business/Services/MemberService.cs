using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AS.CMS.Business.Services
{
    public class MemberService : IMemberService
    {
        #region Repository Injection

        private IBaseRepository<Member> _memberRepository;

        public MemberService(IBaseRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        #endregion

        #region Controller Based Methods

        public PageResultSet<Member> GetActiveMembers(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Member>("Email", pagingFilter.SearchText);
            DetachedCriteria activeMemberPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Member>("Email", pagingFilter.SearchText);

            return new PageResultSet<Member>()
            {
                PageData = _memberRepository.GetWithCriteriaByPaging(activeMemberPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _memberRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Member GetMember(string userName, string password)
        {
            DetachedCriteria membercriteria = CriteriaHelper.GetDefaultCriteria<Member>();
            membercriteria.Add(Expression.Eq("Email", userName));
            membercriteria.Add(Expression.Eq("Password", UtilityHelper.GenerateMD5Hash(password)));

            return _memberRepository.GetWithCriteria(membercriteria, CriteriaHelper.GetDefaultOrder()).FirstOrDefault();
        }

        public Member GetMemberWithID(int memberID)
        {
            return _memberRepository.GetById(memberID);
        }

        public bool SaveMember(Member memberEntity)
        {
            if (memberEntity.ID == 0)
            {
                _memberRepository.Create(memberEntity);
            }
            else
            {
                _memberRepository.Update(memberEntity);
            }

            return true;
        }

        #endregion
    }
}