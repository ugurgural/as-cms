using System.ComponentModel;

namespace AS.CMS.Domain.Common
{
    public enum GenderType
    {
        [Description("")]
        Unknown = 0,
        [Description("Erkek")]
        Male = 1,
        [Description("Kadın")]
        Female = 2
    }
}
