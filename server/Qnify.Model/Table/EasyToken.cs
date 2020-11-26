using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Qnify.Model.Table
{
    [DataContract]
    public class EasyToken
    {
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "uid")]
        public uint UserId { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [DataMember(Name = "un")]
        public string Username { get; set; }

        /// <summary>
        /// EasyTokenValue
        /// </summary>
        [DataMember(Name = "etv")]
        public string EasyTokenValue { get; set; }

        /// <summary>
        /// Expires
        /// </summary>
        [DataMember(Name = "e")]
        public DateTime Expires { get; set; }
    }
}
