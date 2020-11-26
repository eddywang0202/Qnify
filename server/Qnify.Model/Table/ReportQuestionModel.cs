using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class ReportQuestionModel
    {
        /// <summary>
        /// TestQuestionId
        /// </summary>
        [DataMember(Name = "tqid")]
        public uint TestQuestionId { get; set; }

        /// <summary>
        /// QuestionId
        /// </summary>
        [DataMember(Name = "qid")]
        public uint QuestionId { get; set; }

        /// <summary>
        /// CellId
        /// </summary>
        [DataMember(Name = "cid")]
        public uint CellId { get; set; }

        /// <summary>
        /// QuestionTitle
        /// </summary>
        [DataMember(Name = "qt")]
        public string QuestionTitle { get; set; }

        /// <summary>
        /// QuestionType
        /// </summary>
        [DataMember(Name = "qtid")]
        public uint QuestionTypeId { get; set; }

        /// <summary>
        /// QuestionGroupId
        /// </summary>
        [DataMember(Name = "qgid")]
        public uint QuestionGroupId { get; set; }

        /// <summary>
        /// QuestionParentId
        /// </summary>
        [DataMember(Name = "qpid")]
        public uint QuestionParentId { get; set; }

        /// <summary>
        /// QuestionOrder
        /// </summary>
        [DataMember(Name = "qo")]
        public uint QuestionOrder { get; set; }

        /// <summary>
        /// Answers
        /// </summary>
        [DataMember(Name = "a")]
        public IEnumerable<string> Answers { get; set; }

        /// <summary>
        /// CorrectAnswers
        /// </summary>
        [DataMember(Name = "ca")]
        public IEnumerable<string> CorrectAnswers { get; set; }

        /// <summary>
        /// IsCorrect
        /// </summary>
        [DataMember(Name = "ic")]
        public bool IsCorrect { get; set; }

        public ReportQuestionModel()
        {
            Answers = new List<string>();
            CorrectAnswers = new List<string>();
        }
    }
}
