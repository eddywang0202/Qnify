using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class CaseProperty
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }
        [DataMember(Name = "n")]
        public string Name { get; set; }
        [DataMember(Name = "v")]
        public string Value { get; set; }
    }
}
