using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class Cell
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }

        [DataMember(Name = "r")]
        public uint Row { get; set; }

        [DataMember(Name = "c")]
        public uint Column { get; set; }

        [DataMember(Name = "gid")]
        public uint GridId { get; set; }

        [DataMember(Name = "cpjson")]
        public string CellPropertyJson { get; set; }
    }
}
