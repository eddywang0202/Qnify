using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class ParticipantTestQuestionAnswerRequest
    {
        /// <summary>
        /// QuestionId
        /// </summary>
        [DataMember(Name = "tqid")]
        public uint TestQuestionId { get; set; }

        /// <summary>
        /// AnswerIds
        /// </summary>
        [DataMember(Name = "aid")]
        public List<uint> AnswerIds { get; set; }

        public ParticipantTestQuestionAnswerRequest()
        {
            AnswerIds = new List<uint>();
        }
    }
}
