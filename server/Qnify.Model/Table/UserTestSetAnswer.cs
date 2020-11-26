using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class UserTestSetAnswer
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }
        [DataMember(Name = "tsid")]
        public uint TestSetId { get; set; }
        [DataMember(Name = "tsaid")]
        public uint TestSetAnswerId { get; set; }
        [DataMember(Name = "uid")]
        public uint UserId { get; set; }
    }
}
