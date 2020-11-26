using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestSetResponse
    {
        [DataMember(Name = "tsgid")]
        public uint TestSetGroupId { get; set; }

        [DataMember(Name = "a")]
        public List<QuestionModel> QuestionModels { get; set; }

        public TestSetResponse()
        {
            QuestionModels = new List<QuestionModel>();
        }
    }
}
