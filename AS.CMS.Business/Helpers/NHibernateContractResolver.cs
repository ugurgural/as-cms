using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace AS.CMS.Business.Helpers
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(System.Type objectType)
        {
            if (objectType.IsAutoClass && objectType.Namespace == null && typeof(ISerializable).IsAssignableFrom(objectType))
            {
                return base.CreateObjectContract(objectType);
            }

            return base.CreateContract(objectType);
        }
    }
}