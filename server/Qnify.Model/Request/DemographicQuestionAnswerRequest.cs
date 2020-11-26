using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class DemographicQuestionAnswerRequest
    {
        /// <summary>
        /// EasyToken
        /// </summary>
        [DataMember(Name = "et")]
        public string EasyToken { get; set; }

        /// <summary>
        /// UserQuestionAnswers
        /// </summary>
        [DataMember(Name = "uqas")]
        public List<UserQuestionAnswer> UserQuestionAnswers { get; set; }

        public DemographicQuestionAnswerRequest()
        {
            UserQuestionAnswers = new List<UserQuestionAnswer>();
        }

    }
}
