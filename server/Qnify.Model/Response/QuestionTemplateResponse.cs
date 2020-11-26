using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class QuestionTemplateResponse
    {
        /// <summary>
        /// Cells
        /// </summary>
        [DataMember(Name = "c")]
        public List<Cell> Cells { get; set; }

        /// <summary>
        /// QuestionAnswerModels
        /// </summary>
        [DataMember(Name = "q")]
        public List<QuestionModel> QuestionModels { get; set; }

        public QuestionTemplateResponse()
        {
            QuestionModels = new List<QuestionModel>();
            Cells = new List<Cell>();
        }

    }
}
