using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestQuestionResponse
    {
        /// <summary>
        /// TestModels
        /// </summary>
        [DataMember(Name = "t")]
        public List<TestSetModel> TestSetModels { get; set; }

        public TestQuestionResponse()
        {
            TestSetModels = new List<TestSetModel>();
        }
    }
}
