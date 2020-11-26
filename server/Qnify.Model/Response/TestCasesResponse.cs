using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestCasesResponse
    {
        [DataMember(Name = "tcr")]
        public List<TestCaseResponse> TestCaseResponses { get; set; }

        public TestCasesResponse()
        {
            TestCaseResponses = new List<TestCaseResponse>();
        }
    }
}
