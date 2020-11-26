using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class QuestionAnswerResponse
    {
        /// <summary>
        /// Questions
        /// </summary>
        [DataMember(Name = "q")]
        public List<QuestionModel> QuestionModels { get; set; }

        public QuestionAnswerResponse()
        {
            QuestionModels = new List<QuestionModel>();
        }
    }
}
