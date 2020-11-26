using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Qnify.Model.Request
{
    [DataContract]
    public class AuthValidateRequest
    {
        [DataMember(Name = "u")]
        public string Username { get; set; }
        [DataMember(Name = "p")]
        public string Password { get; set; }
    }
}
