using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestSetsResponse
    {
        [DataMember(Name = "tsr")]
        public List<TestSetResponse> TestSetResponses { get; set; }

        public TestSetsResponse()
        {
            TestSetResponses = new List<TestSetResponse>();
        }
    }
}
