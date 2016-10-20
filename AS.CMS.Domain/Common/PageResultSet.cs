using System.Collections.Generic;

namespace AS.CMS.Domain.Common
{
    public class PageResultSet<T>
    {
        public IList<T> PageData { get; set; }
        public int Count { get; set; }
    }
}
