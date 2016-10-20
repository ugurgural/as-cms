using AS.CMS.Domain.Base;
using AS.CMS.Domain.Common;
using System;
using System.Collections.Generic;

namespace AS.CMS.Business.Interfaces
{
    public interface INewsService
    {
        bool SaveNews(News newsEntity);
        PageResultSet<News> GetActiveNews(PagingFilter pagingFilter);
        News GetNewsWithID(int newsID);
    }
}
