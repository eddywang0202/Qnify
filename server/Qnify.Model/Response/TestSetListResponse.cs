using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestSetListResponse
    {
        /// <summary>
        /// TestSetId
        /// </summary>
        [DataMember(Name = "tsid")]
        public uint TestSetId { get; set; }

        /// <summary>
        /// TestSetTitle
        /// </summary>
        [DataMember(Name = "tst")]
        public string TestSetTitle { get; set; }

        /// <summary>
        /// TestSetOrder
        /// </summary>
        [DataMember(Name = "tso")]
        public uint TestSetOrder { get; set; }

        /// <summary>
        /// IsAnswered
        /// </summary>
        [DataMember(Name = "ia")]
        public bool IsAnswered { get; set; }

        /// <summary>
        /// TestId
        /// </summary>
        [DataMember(Name = "tid")]
        public uint TestId { get; set; }
    }
}
