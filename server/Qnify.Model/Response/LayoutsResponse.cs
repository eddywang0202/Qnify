using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class LayoutsResponse
    {
        [DataMember(Name = "l")]
        public List<Layout> Layouts { get; set; }

        public LayoutsResponse()
        {
            Layouts = new List<Layout>();
        }
    }
}
