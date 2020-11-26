using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class QuestionModelRequest
    {
        /// <summary>
        /// CellId
        /// </summary>
        [DataMember(Name = "cid")]
        public uint CellId { get; set; }

        /// <summary>
        /// QuestionId
        /// </summary>
        [DataMember(Name = "qid")]
        public uint QuestionId { get; set; }

        /// <summary>
        /// AnswerIds
        /// </summary>
        [DataMember(Name = "aid")]
        public List<uint> AnswerIds { get; set; }

        public QuestionModelRequest()
        {
            AnswerIds = new List<uint>();
        }
    }
}
