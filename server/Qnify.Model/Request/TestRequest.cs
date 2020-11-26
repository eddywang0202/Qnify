using System.Runtime.Serialization;

namespace Qnify.Model.Request
{
    [DataContract]
    public class TestRequest
    {
        /// <summary>
        /// TestId
        /// </summary>
        [DataMember(Name = "tid")]
        public uint TestId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "t")]
        public string Title { get; set; }

        /// <summary>
        /// Consent
        /// </summary>
        [DataMember(Name = "c")]
        public string Consent { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "s")]
        public bool Status { get; set; }
    }
}
