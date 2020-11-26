using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class TestSetModel
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
        public List<QuestionModel> TestSetQuestionModels { get; set; }

        /// <summary>
        /// QuestionModels
        /// </summary>
        [DataMember(Name = "c")]
        public List<CaseModel> CaseModels { get; set; }

        public TestSetModel()
        {
            CaseModels = new List<CaseModel>();
            TestSetQuestionModels = new List<QuestionModel>();
        }
    }
}
