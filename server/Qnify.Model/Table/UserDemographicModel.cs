using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class UserDemographicModel
    {
        /// <summary>
        /// QuestionTitle
        /// </summary>
        [DataMember(Name = "qt")]
        public string QuestionTitle { get; set; }

        /// <summary>
        /// Answers
        /// </summary>
        [DataMember(Name = "a")]
        public IEnumerable<string> Answers { get; set; }
    }
}
