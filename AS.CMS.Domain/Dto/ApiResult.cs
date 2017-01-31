using System.Runtime.Serialization;

namespace AS.CMS.Domain.Dto
{
    [DataContract]
    public class ApiResult
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Data { get; set; }
    }
}