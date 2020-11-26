using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class AnswerSet
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }
        [DataMember(Name = "qid")]
        public uint QuestionId { get; set; }
        [DataMember(Name = "v")]
        public string Value { get; set; }
        [DataMember(Name = "aaid")]
        public uint AnswerActionId { get; set; }
        [DataMember(Name = "nqid")]
        public uint NextQuestionId { get; set; }
    }
}
