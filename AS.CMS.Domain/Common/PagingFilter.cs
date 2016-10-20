
using System.Collections.Generic;

namespace AS.CMS.Domain.Common
{
    public class PagingFilter
    {
        public PagingFilter()
        {
            PageIndex = 1;
            PageSize = 20;
        }

        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public string SearchText { get; set; }

        //TODO: Her sayfa için aranacak alan farklı, content için Title, member için Email gibi. Kullanıcıyı yönlendirmek için placeholder'a mevcut entity'de hangi alan search için kullanılıyor onu belirtiyoruz.
        public string PlaceHolderText { get; set; }
    }
}
