using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class UserTestCaseAnswer
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }
        [DataMember(Name = "tcid")]
        public uint TestCaseId { get; set; }
        [DataMember(Name = "tcaid")]
        public uint TestCaseAnswerId { get; set; }
        [DataMember(Name = "uid")]
        public uint UserId { get; set; }
    }
}
