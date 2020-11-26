using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class UserTestSetAnswersResponse
    {
        [DataMember(Name = "utsa")]
        public List<UserTestSetAnswer> UserTestSetAnswers { get; set; }

        public UserTestSetAnswersResponse()
        {
            UserTestSetAnswers = new List<UserTestSetAnswer>();
        }
    }
}
