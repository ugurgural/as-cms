using System.ComponentModel;

namespace AS.CMS.Domain.Common
{
    public enum ModalStatus
    {
        [Description("Başarılı")]
        Success = 1,
        [Description("Hata")]
        Error = 2,
        [Description("Uyarı")]
        Warning = 3,
        [Description("Bilgilendirme")]
        Info = 4
    }
}
