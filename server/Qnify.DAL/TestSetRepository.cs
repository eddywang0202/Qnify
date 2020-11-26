using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class TestSetRepository : BaseRepository
    {
        public TestSetRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public uint GetTestId(List<uint> testQuestionIds)
        {
            const string commandText =
@"SELECT test_id FROM test_set
WHERE id IN (SELECT test_set_id
FROM test_question
WHERE id IN @TestQuestionIds);";
            var param = new DynamicParameters();
            param.Add("@TestQuestionIds", testQuestionIds);

            return Conn.Query<uint>(commandText, param).FirstOrDefault();
        }

        public uint Insert(string title, uint order, uint testId)
        {
            const string commandText =
@"INSERT INTO `test_set`
(`title`,`order`,`test_id`)
VALUES
(@Title,@Order,@TestId);
select LAST_INSERT_ID();";
            var param = new DynamicParameters();
            param.Add("@Title", title);
            param.Add("@Order", order);
            param.Add("@TestId", testId);

            return Conn.Query<uint>(commandText, param).FirstOrDefault();            
        }

        public bool Update(string title, uint testSetId)
        {
            const string commandText =
@"UPDATE test_set
SET title = @Title
WHERE id = @TestSetId;";
            var param = new DynamicParameters();
            param.Add("@Title", title);
            param.Add("@TestSetId", testSetId);

            return Conn.Execute(commandText, param) > 0;
        }

        public uint? GetMaxOrderId()
        {
            const string commandText =
@"SELECT MAX(`order`) FROM `test_set`";

            return Conn.Query<uint?>(commandText).FirstOrDefault();
        }
    }
}
