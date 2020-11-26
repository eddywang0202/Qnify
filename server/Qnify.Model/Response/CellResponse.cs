using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class CellResponse
    {
        [DataMember(Name = "c")]
        public List<Cell> Cells { get; set; }

        public CellResponse()
        {
            Cells = new List<Cell>();
        }
    }
}
