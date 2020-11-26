using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class AnswerActionsResponse
    {
        [DataMember(Name = "aa")]
        public List<AnswerAction> AnswerActions { get; set; }

        public AnswerActionsResponse()
        {
            AnswerActions = new List<AnswerAction>();
        }
    }
}
