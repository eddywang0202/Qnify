using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class UserTestQuestionRequest
    {
        /// <summary>
        /// UserQuestionAnswers
        /// </summary>
        [DataMember(Name = "uqas")]
        public List<UserQuestionAnswer> UserQuestionAnswers { get; set; }

        public UserTestQuestionRequest()
        {
            UserQuestionAnswers = new List<UserQuestionAnswer>();
        }

    }
}
