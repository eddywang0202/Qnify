using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class AnswerAction
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }
        [DataMember(Name = "v")]
        public string Value { get; set; }
        [DataMember(Name = "vd")]
        public string ValueDetail { get; set; }
    }
}
