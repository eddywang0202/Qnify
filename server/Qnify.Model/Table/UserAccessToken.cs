using System;

namespace Qnify.Model
{
    public class UserAccessToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedTimestamp { get; set; }
        public virtual User User { get; set; }
    }
}
