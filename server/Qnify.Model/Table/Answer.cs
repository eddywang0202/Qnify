using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class Answer
    {
        /// <summary>
        /// AnswerId
        /// </summary>
        [DataMember(Name = "aid")]
        public uint AnswerId { get; set; }

        /// <summary>
        /// AnswerTitle
        /// </summary>
        [DataMember(Name = "at")]
        public string AnswerTitle { get; set; }

        /// <summary>
        /// NextQuestionId
        /// </summary>
        [DataMember(Name = "nqid")]
        public uint NextQuestionId { get; set; }

        /// <summary>
        /// AnswerOrder
        /// </summary>
        [DataMember(Name = "ao")]
        public uint AnswerOrder { get; set; }

        /// <summary>
        /// IsChosenAnswer
        /// </summary>
        [DataMember(Name = "ica")]
        public bool IsChosenAnswer { get; set; }
    }
}
