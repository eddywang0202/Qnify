using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class Test
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
        /// Status
        /// </summary>
        [DataMember(Name = "s")]
        public bool Status { get; set; }
     
        public string Consent { get; set; }
    }
}
