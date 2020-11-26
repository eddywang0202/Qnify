using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestSet
    {
        [DataMember(Name = "tsgid")]
        public uint TestSetGroupId { get; set; }
        [DataMember(Name = "qid")]
        public uint QuestionId { get; set; }
        [DataMember(Name = "q")]
        public string QuestionValue { get; set; }
        [DataMember(Name = "qpid")]
        public uint QuestionParentId { get; set; }
        [DataMember(Name = "a")]
        public string AnswerValue { get; set; }
        [DataMember(Name = "aa")]
        public string AnswerActionValue { get; set; }
        [DataMember(Name = "nqid")]
        public uint NextQuestionId { get; set; }
        [DataMember(Name = "at")]
        public string AnswerType { get; set; }
    }
}
