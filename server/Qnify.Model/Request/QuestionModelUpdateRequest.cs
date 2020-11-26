using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class QuestionModelUpdateRequest : QuestionModelRequest
    {
        /// <summary>
        /// TestQuestionId
        /// </summary>
        [DataMember(Name = "tqid")]
        public uint TestQuestionId { get; set; }
    }
}
