using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class TestCaseCase
    {
        [DataMember(Name = "cid")]
        public uint CaseId { get; set; }

        [DataMember(Name = "a")]
        public List<QuestionModel> QuestionModels { get; set; }

        public TestCaseCase()
        {
            QuestionModels = new List<QuestionModel>();
        }
    }
}
