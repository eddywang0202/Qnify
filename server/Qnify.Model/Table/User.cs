using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }
        [DataMember(Name = "tid")]
        public uint TestId { get; set; }
        [DataMember(Name = "un")]
        public string Username { get; set; }
        [DataMember(Name = "r")]
        public string Role { get; set; }        
        public uint RowNumber { get; set; }
    }
}
