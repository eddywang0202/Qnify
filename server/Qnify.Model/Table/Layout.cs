using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class Layout
    {
        [DataMember(Name = "r")]
        public string Row { get; set; }
        [DataMember(Name = "l")]
        public string Column { get; set; }
    }
}
