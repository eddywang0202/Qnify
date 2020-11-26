using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class CaseModel
    {
        /// <summary>
        /// CellId
        /// </summary>
        [DataMember(Name = "cid")]
        public uint CellId { get; set; }

        /// <summary>
        /// CellRow
        /// </summary>
        [DataMember(Name = "cellr")]
        public uint CellRow { get; set; }

        /// <summary>
        /// CellColumn
        /// </summary>
        [DataMember(Name = "celll")]
        public uint CellColumn { get; set; }

        /// <summary>
        /// CellImage
        /// </summary>
        [DataMember(Name = "cimg")]
        public string CellImage { get; set; }

        /// <summary>
        /// CellPropertyJson
        /// </summary>
        [DataMember(Name = "cpjson")]
        public string CellPropertyJson { get; set; }

        /// <summary>
        /// AdditionalCellPropertyJson
        /// </summary>
        [DataMember(Name = "acpjson")]
        public string AdditionalCellPropertyJson { get; set; }

        /// <summary>
        /// QuestionModels
        /// </summary>
        //[DataMember(Name = "tq")]
        //public List<QuestionModel> QuestionModels { get; set; }

        //public CaseModel()
        //{
        //    QuestionModels = new List<QuestionModel>();
        //}
    }
}
