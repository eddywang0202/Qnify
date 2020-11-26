using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class CasesResponse
    {
        [DataMember(Name = "c")]
        public List<Case> Cases { get; set; }

        public CasesResponse()
        {
            Cases = new List<Case>();
        }
    }
}
