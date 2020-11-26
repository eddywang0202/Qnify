using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class CaseModelRequest
    {
        /// <summary>
        /// CellId
        /// </summary>
        [DataMember(Name = "cid")]
        public uint CellId { get; set; }

        /// <summary>
        /// CellImage
        /// </summary>
        [DataMember(Name = "cimg")]
        public string CellImage { get; set; }

        /// <summary>
        /// AdditionalCellPropertyJson
        /// </summary>
        [DataMember(Name = "acpjson")]
        public string AdditionalCellPropertyJson { get; set; }
    }
}
