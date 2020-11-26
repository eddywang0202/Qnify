using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestCase : TestSet
    {
        [DataMember(Name = "cid")]
        public uint CaseId { get; set; }
    }
}
