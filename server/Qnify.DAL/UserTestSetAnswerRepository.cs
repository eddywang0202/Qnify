using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class UserTestSetAnswerRepository : BaseRepository
    {
        public UserTestSetAnswerRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<UserTestSetAnswer> GetUserTestSetAnswers()
        {
            const string commandText =
@"SELECT `id`,
`test_set_id` as TestSetId,
`test_set_answer_id` as TestSetAnswerId,
`user_id` as UserId
FROM `user_test_set_answer`";

            var result = Conn.Query<UserTestSetAnswer>(commandText).ToList();
            return result;
        }

        public bool Insert(uint testSetId, uint testSetAnswerId, uint userId)
        {
            const string commandText =
@"INSERT INTO `qnify`.`user_test_set_answer`(`test_set_id`,`test_set_answer_id`,`user_id`)
VALUES(@TestSetId,@TestSetAnswerId,@UserId)
";

            var param = new DynamicParameters();
            param.Add("@TestSetId", testSetId);
            param.Add("@TestSetAnswerId", testSetAnswerId);
            param.Add("@UserId", userId);
            var result = Conn.Execute(commandText, param) > 0;
            return result;
        }
    }
}
