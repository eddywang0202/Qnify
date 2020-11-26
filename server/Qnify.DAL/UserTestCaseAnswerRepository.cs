using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class UserTestCaseAnswerRepository : BaseRepository
    {
        public UserTestCaseAnswerRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<UserTestCaseAnswer> GetUserTestCaseAnswers()
        {
            const string commandText =
@"SELECT `id`,
`test_case_id` as TestCaseId,
`test_case_answer_id` as TestCaseAnswerId,
`user_id` as UserId
FROM `user_test_case_answer`";

            var result = Conn.Query<UserTestCaseAnswer>(commandText).ToList();
            return result;
        }

        public bool Insert(uint testCaseId, uint testCaseAnswerId, uint userId)
        {
            const string commandText =
@"INSERT INTO `qnify`.`user_test_case_answer`(`test_case_id`,`test_case_answer_id`,`user_id`)
VALUES(@TestCaseId,@TestCaseAnswerId,@UserId)
";

            var param = new DynamicParameters();
            param.Add("@TestCaseId", testCaseId);
            param.Add("@TestCaseAnswerId", testCaseAnswerId);
            param.Add("@UserId", userId);
            var result = Conn.Execute(commandText, param) > 0;
            return result;
        }
    }
}
