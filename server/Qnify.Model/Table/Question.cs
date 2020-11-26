using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class Question
    {
        public uint TestSetId { get; set; }
        public uint TestId { get; set; }
        public uint CaseId { get; set; }
        public string CasePropertyTitle { get; set; }
        public uint TestQuestionId { get; set; }
        public uint QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public uint QuestionTypeId { get; set; }
        public uint QuestionGroupId { get; set; }
        public uint QuestionParentId { get; set; }
        public uint QuestionOrder { get; set; }
        public uint AnswerId { get; set; }
        public string AnswerTitle { get; set; }
        public uint AnswerNextQuestionId { get; set; }
        public uint AnswerOrder { get; set; }
    }
}
