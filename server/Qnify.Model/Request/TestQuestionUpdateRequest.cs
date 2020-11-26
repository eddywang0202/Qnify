using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class TestQuestionUpdateRequest
    {
        /// <summary>
        /// TestSetId
        /// </summary>
        [DataMember(Name = "tsid")]
        public uint TestSetId { get; set; }

        /// <summary>
        /// TestSetTitle
        /// </summary>
        [DataMember(Name = "tst")]
        public string TestSetTitle { get; set; }

        /// <summary>
        /// TestSetQuestionModels
        /// </summary>
        [DataMember(Name = "tsq")]
        public List<QuestionModelUpdateRequest> TestSetQuestionModels { get; set; }

        /// <summary>
        /// CaseModels
        /// </summary>
        [DataMember(Name = "c")]
        public List<CaseModelUpdateRequest> CaseModels { get; set; }

        public TestQuestionUpdateRequest()
        {
            TestSetQuestionModels = new List<QuestionModelUpdateRequest>();
            CaseModels = new List<CaseModelUpdateRequest>();
        }
    }
}
