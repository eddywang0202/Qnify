using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class TestQuestionRequest
    {
        /// <summary>
        /// TestId
        /// </summary>
        [DataMember(Name = "tid")]
        public uint TestId { get; set; }

        /// <summary>
        /// TestSetTitle
        /// </summary>
        [DataMember(Name = "tst")]
        public string TestSetTitle { get; set; }

        /// <summary>
        /// TestSetQuestionModels
        /// </summary>
        [DataMember(Name = "tsq")]
        public List<QuestionModelRequest> TestSetQuestionModels { get; set; }

        /// <summary>
        /// CaseModels
        /// </summary>
        [DataMember(Name = "c")]
        public List<CaseModelRequest> CaseModels { get; set; }

        public TestQuestionRequest()
        {
            TestSetQuestionModels = new List<QuestionModelRequest>();
            CaseModels = new List<CaseModelRequest>();
        }
    }
}
