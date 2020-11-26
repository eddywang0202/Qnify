using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class ReportDetail
    {
        /// <summary>
        /// TestQuestionId
        /// </summary>
        [DataMember(Name = "tqid")]
        public uint TestQuestionId { get; set; }

        /// <summary>
        /// QuestionTitle
        /// </summary>
        [DataMember(Name = "qt")]
        public string QuestionTitle { get; set; }

        /// <summary>
        /// CellImage
        /// </summary>
        [DataMember(Name = "cimg")]
        public string CellImage { get; set; }

        /// <summary>
        /// ChoosenAnswers
        /// </summary>
        [DataMember(Name = "ca")]
        public IEnumerable<string> ChoosenAnswers { get; set; }

        public ReportDetail()
        {
            ChoosenAnswers = new List<string>();
        }
    }
}
