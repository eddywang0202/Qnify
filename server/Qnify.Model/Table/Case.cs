using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class Case
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "id")]
        public uint Id { get; set; }

        /// <summary>
        /// CellId
        /// </summary>
        [DataMember(Name = "cid")]
        public uint CellId { get; set; }

        /// <summary>
        /// AdtCellPropJson
        /// </summary>
        [DataMember(Name = "acpj")]
        public string AdtCellPropJson { get; set; }

        /// <summary>
        /// ImageName
        /// </summary>
        [DataMember(Name = "in")]
        public string ImageName { get; set; }

        /// <summary>
        /// ImageName
        /// </summary>
        [DataMember(Name = "qcas")]
        public List<QuestionCorrectAnswer> QuestionCorrectAnswers { get; set; }

        public Case()
        {
            QuestionCorrectAnswers = new List<QuestionCorrectAnswer>();
        }
    }
}
