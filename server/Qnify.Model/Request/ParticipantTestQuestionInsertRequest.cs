using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class ParticipantTestQuestionInsertRequest
    {
        /// <summary>
        /// UserQuestionAnswers
        /// </summary>
        [DataMember(Name = "uqas")]
        public List<ParticipantTestQuestionAnswerRequest> UserQuestionAnswers { get; set; }

        public ParticipantTestQuestionInsertRequest()
        {
            UserQuestionAnswers = new List<ParticipantTestQuestionAnswerRequest>();
        }
    }
}
