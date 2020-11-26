using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestCasePropertiesResponse
    {
        [DataMember(Name = "tcp")]
        public List<TestCaseProperty> TestCaseProperties { get; set; }

        public TestCasePropertiesResponse()
        {
            TestCaseProperties = new List<TestCaseProperty>();
        }
    }
}
