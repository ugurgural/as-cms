using AS.CMS.Domain.Interfaces;
using FluentNHibernate.Automapping;
using System;
using System.Collections.Generic;

namespace AS.CMS.Data.UOW.Helpers
{
    public class AutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        private static readonly IList<string> IgnoredMembers = new List<string> { "CurrentPage", "TotalPage" };

        public override bool ShouldMap(Type type)
        {
            return type.GetInterface(typeof(ICMSEntity).FullName) != null && !IgnoredMembers.Contains(type.Name);
        }
    }
}
