using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class UsersResponse
    {
        [DataMember(Name = "us")]
        public List<User> Users { get; set; }

        public UsersResponse()
        {
            Users = new List<User>();
        }
    }
}
