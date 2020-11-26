using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    public class DemographicQuestion: QuestionModel { }

    [DataContract]
    public class QuestionResponse
    {
        /// <summary>
        /// Questions
        /// </summary>
        [DataMember(Name = "qs")]
        public List<DemographicQuestion> QuestionModels { get; set; }

        public QuestionResponse()
        {
            QuestionModels = new List<DemographicQuestion>();
        }
    }
}
