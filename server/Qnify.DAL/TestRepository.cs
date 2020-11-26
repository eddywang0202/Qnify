using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class TestRepository : BaseRepository
    {
        public TestRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public bool UpdateConsent(uint testId, string consent)
        {
            const string commandText =
@"UPDATE `test`
SET consent = @Consent
WHERE id = @TestId;";
            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            param.Add("@Consent", consent);

            return Conn.Execute(commandText, param) > 0;
        }

        public bool UpdateTitle(uint testId, string title)
        {
            const string commandText =
@"UPDATE `test`
SET title = @Title
WHERE id = @TestId;";
            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            param.Add("@Title", title);

            return Conn.Execute(commandText, param) > 0;
        }

        public bool UpdateStatus(uint testId, bool status)
        {
            const string commandText =
@"UPDATE `test`
SET `status` = @Status
WHERE id = @TestId;";
            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            param.Add("@Status", status);

            return Conn.Execute(commandText, param) > 0;
        }

        public bool UpdateFirstTestStatusToActive()
        {
            const string commandText =
@"UPDATE `test`
SET `status` = @Status
WHERE id = @TestId;";
            var param = new DynamicParameters();
            param.Add("@TestId", 1);
            param.Add("@Status", 1);

            return Conn.Execute(commandText, param) > 0;
        }

        public bool Update(uint testId, string title, string consent, bool status)
        {
            const string commandText =
@"UPDATE `test`
SET
title = @Title,
consent = @Consent,
status = @Status
WHERE id = @TestId;";
            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            param.Add("@Title", title);
            param.Add("@Consent", consent);
            param.Add("@Status", status);

            return Conn.Execute(commandText, param) > 0;
        }        

        public uint Insert(string title, string consent)
        {
            const string commandText =
@"INSERT INTO test
(`title`,`consent`)
VALUES
(@Title, @Consent);
select LAST_INSERT_ID();";

            var param = new DynamicParameters();            
            param.Add("@Title", title);
            param.Add("@Consent", consent);

            return Conn.Query<uint>(commandText, param).FirstOrDefault();
        }

        public uint InsertTestWithTitle(string title)
        {
            const string commandText =
@"INSERT INTO test
(`title`,`status`)
VALUES
(@Title,@Status);
select LAST_INSERT_ID();";

            var param = new DynamicParameters();
            param.Add("@Title", title);
            param.Add("@Status", 0);

            return Conn.Query<uint>(commandText, param).FirstOrDefault();
        }

        public IEnumerable<Test> GetTests()
        {
            const string commandText =
@"SELECT *, id As TestId FROM `test`";
            return Conn.Query<Test>(commandText).ToList();
        }

        public Test GetTest(uint testId)
        {
            const string commandText =
@"SELECT *, id As TestId FROM `test` WHERE id = @TestId";

            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            return Conn.QueryFirstOrDefault<Test>(commandText, param);
        }

        public Test GetActiveTest()
        {
            const string commandText =
@"SELECT *, id As TestId FROM `test` WHERE `status` = @Status;";

            var param = new DynamicParameters();
            param.Add("@Status", 1);
            return Conn.QueryFirstOrDefault<Test>(commandText, param);
        }

        public uint GetActiveTestId()
        {
            const string commandText =
@"SELECT id FROM `test` WHERE `status` = @Status;";

            var param = new DynamicParameters();
            param.Add("@Status", 1);
            return Conn.QueryFirstOrDefault<uint>(commandText, param);
        }

        public IEnumerable<uint> GetActiveTestIds()
        {
            const string commandText =
@"SELECT id FROM `test` WHERE `status` = @Status;";

            var param = new DynamicParameters();
            param.Add("@Status", 1);
            return Conn.Query<uint>(commandText, param).ToList();
        }

        public Test GetTest(string title)
        {
            const string commandText =
@"SELECT *, id As TestId FROM `test` WHERE title = @Title";

            var param = new DynamicParameters();
            param.Add("@Title", title);
            return Conn.QueryFirstOrDefault<Test>(commandText, param);
        }

        public bool Delete(uint testId)
        {
            const string commandText =
@"DELETE FROM `test` WHERE id = @TestId";

            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            return Conn.Execute(commandText, param) > 0;
        }

        public bool UpdateAllStatusToFalse(uint testId)
        {
            const string commandText =
@"UPDATE test
SET `status` = @Status
WHERE id != @TestId;";

            var param = new DynamicParameters();
            param.Add("@Status", 0);
            param.Add("@TestId", testId);
            return Conn.Execute(commandText, param) > 0;
        }
    }
}
