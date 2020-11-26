using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestCaseResponse
    {
        [DataMember(Name = "tsgid")]
        public uint TestSetGroupId { get; set; }

        [DataMember(Name = "a")]
        public List<TestCaseCase> TestCaseCases { get; set; }

        public TestCaseResponse()
        {
            TestCaseCases = new List<TestCaseCase>();
        }
    }
}
