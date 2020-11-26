using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class GetUserTestQuestionReportsRequest
    {
        /// <summary>
        /// Offset
        /// </summary>
        [DataMember(Name = "o")]
        public int Offset { get; set; }

        /// <summary>
        /// Limit
        /// </summary>
        [DataMember(Name = "l")]
        public int Limit { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [DataMember(Name = "un")]
        public string Username { get; set; }

        /// <summary>
        /// TestId
        /// </summary>
        [DataMember(Name = "tid")]
        public uint? TestId { get; set; }
    }
}
