using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class AnswerSetsResponse
    {
        [DataMember(Name = "a")]
        public List<AnswerSet> AnswerSets { get; set; }

        public AnswerSetsResponse()
        {
            AnswerSets = new List<AnswerSet>();
        }
    }
}
