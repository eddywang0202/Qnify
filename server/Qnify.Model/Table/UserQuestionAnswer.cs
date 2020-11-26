using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Qnify.Model.Table
{
    [DataContract]
    public class UserQuestionAnswer
    {
        public uint UserId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// TestQuestionId
        /// </summary>
        [DataMember(Name = "tqid")]
        public uint TestQuestionId { get; set; }

        /// <summary>
        /// ChoosenAnswer
        /// </summary>
        [DataMember(Name = "a")]
        public string ChoosenAnswer { get; set; }

        public uint TestId { get; set; }
    }
}
