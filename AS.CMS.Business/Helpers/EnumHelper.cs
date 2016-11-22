using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace AS.CMS.Business.Helpers
{
    public static class EnumHelper
    {
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }

        public static IEnumerable<SelectListItem> EnumToSelectList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<T>().Select(
                enumItem => new SelectListItem() { Text = ((int)(object)enumItem).ToString(), Value = enumItem.ToString() })).ToList();
        }
    }
}
