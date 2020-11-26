using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class UserTestCaseAnswersResponse
    {
        [DataMember(Name = "utca")]
        public List<UserTestCaseAnswer> UserTestCaseAnswers { get; set; }

        public UserTestCaseAnswersResponse()
        {
            UserTestCaseAnswers = new List<UserTestCaseAnswer>();
        }
    }
}
