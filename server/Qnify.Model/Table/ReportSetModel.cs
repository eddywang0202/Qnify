using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class ReportSetModel
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
        public List<ReportQuestionModel> ReportQuestionModels { get; set; }

        /// <summary>
        /// QuestionModels
        /// </summary>
        [DataMember(Name = "c")]
        public List<CaseModel> CaseModels { get; set; }

        public ReportSetModel()
        {
            CaseModels = new List<CaseModel>();
            ReportQuestionModels = new List<ReportQuestionModel>();
        }
    }
}
