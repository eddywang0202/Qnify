using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Qnify.DAL
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<User> GetUsers()
        {
            const string commandText =
@"SELECT *
FROM `user`";

            var result = Conn.Query<User>(commandText).ToList();
            return result;
        }

        public List<User> GetMemberUsers()
        {
            var commandText = new StringBuilder();

            commandText.Append(@"SET sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY',''));
                                SELECT *
                                FROM `user` a
                                INNER JOIN
                                (
                                SELECT test_question_id, user_id, test_id as TestId, ROW_NUMBER() OVER( ORDER BY created_datetime DESC ) AS 'rownumber'
                                FROM `user_test_question_answer`
                                WHERE test_question_id NOT IN (SELECT id FROM test_question WHERE question_id IN (SELECT id FROM question WHERE question_group_id = 1))
                                AND test_id IN (SELECT id FROM test)
                                GROUP BY created_datetime
                                ORDER BY created_datetime DESC
                                ) b
                                ON a.id = b.user_id
                                GROUP BY TestId, user_id
                                ORDER BY rownumber");

            var result = Conn.Query<User>(commandText.ToString()).ToList();
            return result;
        }

        public List<User> GetMemberUsers(uint testId)
        {
            var commandText = new StringBuilder();

            commandText.Append(@"SET sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY',''));
                                SELECT *
                                FROM `user` a
                                INNER JOIN
                                (
                                SELECT test_question_id, user_id, test_id as TestId, ROW_NUMBER() OVER( ORDER BY created_datetime DESC ) AS 'rownumber'
                                FROM `user_test_question_answer`
                                WHERE test_question_id NOT IN (SELECT id FROM test_question WHERE question_id IN (SELECT id FROM question WHERE question_group_id = 1))
                                AND test_id IN (SELECT id FROM test)
                                GROUP BY created_datetime
                                ORDER BY created_datetime DESC
                                ) b
                                ON a.id = b.user_id");

            var param = new DynamicParameters();

            if (testId > 0)
            {                
                commandText.Append(" WHERE TestId = @TestId");
                param.Add("@TestId", testId);
            }

            commandText.Append(@" GROUP BY TestId, user_id
                                    ORDER BY rownumber");

            var result = Conn.Query<User>(commandText.ToString(), param).ToList();
            return result;
        }

        public User GetMemberUser(string userName)
        {
            const string commandText =
@"SELECT * FROM `user` WHERE `role` = @Role AND username = @Username";

            var param = new DynamicParameters();
            param.Add("@Role", DefaultValue.Role);
            param.Add("@Username", userName);
            var result = Conn.QueryFirstOrDefault<User>(commandText, param);
            return result;
        }

        public User GetMemberUser(uint userId)
        {
            const string commandText =
@"SELECT * FROM `user` WHERE `role` = @Role AND id = @UserId";

            var param = new DynamicParameters();
            param.Add("@Role", DefaultValue.Role);
            param.Add("@UserId", userId);
            var result = Conn.QueryFirstOrDefault<User>(commandText, param);
            return result;
        }

        public User GetUser(string username, string password)
        {
            var ctx = new
            {
                Username = username,
                Password = password
            };

            string query = $"SELECT * FROM `user` WHERE `username` = @{nameof(ctx.Username)} AND `password` = @{nameof(ctx.Password)}";
            var result = Conn.Query<User>(query, ctx);
            return result.FirstOrDefault();
        }

        public uint InsertUser(string username)
        {           
            const string commandText =
@"INSERT INTO `user`
(`username`,`role`)
VALUES
(@Username, @Role);
select LAST_INSERT_ID();";
            var param = new DynamicParameters();
            param.Add("@Username", username);
            param.Add("@Role", DefaultValue.Role);

            return Conn.Query<uint>(commandText, param).FirstOrDefault();            
        }
    }
}
