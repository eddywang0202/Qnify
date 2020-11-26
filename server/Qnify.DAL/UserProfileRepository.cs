using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class UserProfileRepository : BaseRepository
    {
        public UserProfileRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public bool InsertUserProfile(string name, string age, string email, uint userId)
        {
            const string commandText =
@"INSERT INTO `user_profile`
(`name`,`age`,`email`,`user_id`)
VALUES
(@Name,@Age,@Email,@UserId);";
            var param = new DynamicParameters();
            param.Add("@Name", name);
            param.Add("@Age", age);
            param.Add("@Email", email);
            param.Add("@UserId", userId);
            return Conn.Execute(commandText, param) > 0;
        }
    }
}
