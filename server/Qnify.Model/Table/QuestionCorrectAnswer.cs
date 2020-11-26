using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class QuestionCorrectAnswer
    {
        /// <summary>
        /// QuestionAnswerId
        /// </summary>
        [DataMember(Name = "qid")]
        public uint QuestionId { get; set; }

        /// <summary>
        /// AnswerTitle
        /// </summary>
        [DataMember(Name = "caid")]
        public List<uint> CorrectAnswerId { get; set; }
    }
}
