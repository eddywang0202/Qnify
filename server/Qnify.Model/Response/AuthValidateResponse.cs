using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Qnify.Model.Response
{
    public class AuthValidateResponse
    {
        [DataMember(Name = "atk")]
        public string AccessToken { get; set; }
    }
}
