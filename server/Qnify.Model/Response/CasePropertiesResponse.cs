using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class CasePropertiesResponse
    {
        [DataMember(Name = "cp")]
        public List<CaseProperty> CaseProperties { get; set; }

        public CasePropertiesResponse()
        {
            CaseProperties = new List<CaseProperty>();
        }
    }
}
