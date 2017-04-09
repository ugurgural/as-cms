using System.ComponentModel;

namespace AS.CMS.Domain.Common
{
    public enum EmployeeStatus
    {
        [Description("Onaylanmamış")]
        NotActive = 0,
        [Description("Onaylanmış")]
        Active = 1,
        [Description("Silinmiş")]
        Deleted = 2,
    }
}
