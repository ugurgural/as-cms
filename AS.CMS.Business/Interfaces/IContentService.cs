using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;

namespace AS.CMS.Business.Interfaces
{
    public interface IContentService
    {
        bool SaveContent(Content contentEntity);
        PageResultSet<Content> GetActiveContents(PagingFilter pagingFilter);
        Content GetContentWithID(int contentID);
    }
}