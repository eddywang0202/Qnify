using Dapper;
using Qnify.Model.Table;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Qnify.DAL
{
    public class UserTestQuestionAnswerRepository : BaseRepository
    {
        public UserTestQuestionAnswerRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public bool InsertOrUpodateUserTestQuestionAnswer(uint questionId, string choosenAnswer, uint userId, uint testId)
        {
            const string commandText =
@"INSERT INTO `user_test_question_answer`
(`test_question_id`,`choosen_answer`,`user_id`,`test_id`)
VALUES
(@TestQuestionId, @ChoosenAnswer, @UserId, @TestId);";
            var param = new DynamicParameters();
            param.Add("@TestQuestionId", questionId);
            param.Add("@ChoosenAnswer", choosenAnswer);
            param.Add("@UserId", userId);
            param.Add("@TestId", testId);
            return Conn.Execute(commandText, param) > 0;   
        }

        public List<UserQuestionAnswer> GetUserTestQuestionAnswer(uint userId)
        {
            const string commandText =
@"SELECT
user_id as UserId,
test_id as TestId,
test_question_id as TestQuestionId,
choosen_answer as ChoosenAnswer,
created_datetime as CreatedDateTime
FROM user_test_question_answer
WHERE user_id = @UserId;";
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            return Conn.Query<UserQuestionAnswer>(commandText, param).ToList();
        }

        public List<UserQuestionAnswer> GetUserTestQuestionAnswer()
        {
            const string commandText =
@"SELECT
user_id as UserId,
test_id as TestId,
test_question_id as TestQuestionId,
choosen_answer as ChoosenAnswer,
created_datetime as CreatedDateTime
FROM user_test_question_answer;";

            return Conn.Query<UserQuestionAnswer>(commandText).ToList();
        }

        public List<UserQuestionAnswer> GetUsersTestQuestionAnswer(List<uint> userIds)
        {
            const string commandText =
@"SELECT
user_id as UserId,
test_id as TestId,
test_question_id as TestQuestionId,
choosen_answer as ChoosenAnswer,
created_datetime as CreatedDateTime
FROM user_test_question_answer
WHERE user_id in @UserIds;";
            var param = new DynamicParameters();
            param.Add("@UserIds", userIds);
            return Conn.Query<UserQuestionAnswer>(commandText, param).ToList();
        }

        public bool DeleteUserTestQuestionAnswer(uint questionId, uint userId)
        {
            const string commandText =
@"DELETE FROM `user_test_question_answer` WHERE `user_id` = @UserId && `test_question_id` = @TestQuestionId;";
            var param = new DynamicParameters();
            param.Add("@TestQuestionId", questionId);            
            param.Add("@UserId", userId);
            return Conn.Execute(commandText, param) > 0;
        }
    }
}
