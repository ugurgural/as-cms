using AS.CMS.Domain.Interfaces;
using System;

namespace AS.CMS.Domain.Base
{
    public class Announcement : EntityBase, ICMSEntity
    {
        public virtual string Title { get; set; }
        public virtual string Permalink { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual string Description { get; set; }
        public virtual string SeoTitle { get; set; }
        public virtual string SeoDescription { get; set; }
        public virtual string SeoKeyword { get; set; }
        public virtual string EditorImageList { get; set; }
        public virtual string ImageGallery { get; set; }
        public virtual bool IsPublished { get; set; }
    }
}
