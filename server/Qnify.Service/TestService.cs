using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class TestService : ITestService
    {
        private static Database _database;

        public TestService()
        {
            _database = new Database(Config.AppSettings);
        }

        public string GetConsent(uint testId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                var test = new TestRepository(unitOfWork).GetTest(testId);
                return (test == null || string.IsNullOrEmpty(test.Consent)) ? "" : test.Consent;
            }
        }

        public IEnumerable<Test> GetTests()
        {
            IEnumerable<Test> response;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new TestRepository(unitOfWork).GetTests();
            }
            return response;
        }

        public Test GetTest(uint testId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).GetTest(testId);
            }
        }

        public Test GetActiveTest()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).GetActiveTest();
            }
        }

        public uint GetActiveTestId()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).GetActiveTestId();
            }
        }

        public IEnumerable<uint> GetActiveTestIds()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).GetActiveTestIds();
            }
        }

        public bool Delete(uint testId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).Delete(testId);
            }
        }

        public bool UpdateConsent(uint testId, string consent)
        {
            var response = false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new TestRepository(unitOfWork).UpdateConsent(testId, consent);
            }
            return response;
        }

        public bool UpdateTitle(uint testId, string title)
        {
            var response = false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new TestRepository(unitOfWork).UpdateTitle(testId, title);
            }
            return response;
        }

        public Test UpdateStatus(uint testId, bool status)
        {
            var response = new Test();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                new TestRepository(unitOfWork).UpdateStatus(testId, status);
            }

            response = ProcessUpdateTestStatus(testId, status);

            return response;
        }

        public Test InsertOrUpdateTest(uint testId, string title, string consent, bool status)
        {            
            if (testId > 0)
            {
                if (CheckDuplicateTitleExist(title, testId)) return null;
                Update(testId, title, consent, status);
                return GetTest(testId);
            }
            else
            {
                if (CheckDuplicateTitleExist(title, testId)) return null;
                var newTestId = Insert(title, consent);
                return GetTest(newTestId);
            }            
        }

        public Test Insert(string title)
        {
            if (CheckDuplicateTitleExist(title)) return null;
            var NewTestId = InsertTestWithTitle(title);
            return GetTest(NewTestId);
        }

        #region Private Method

        private Test ProcessUpdateTestStatus(uint testId, bool status)
        {
            var response = new Test();

            if (status)
                UpdateAllStatusToFalse(testId);
            else
            {
                var activeTest = GetActiveTest();
                if (activeTest == null) return response;
            }
            return GetTest(testId);
        }

        private bool Update(uint testId, string title, string consent, bool status)
        {
            var response = false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new TestRepository(unitOfWork).Update(testId, title, consent, status);
            }
            return response;
        }

        private bool UpdateAllStatusToFalse(uint testId)
        {
            var response = false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new TestRepository(unitOfWork).UpdateAllStatusToFalse(testId);
            }
            return response;
        }

        private bool UpdateFirstTestStatusToActive()
        {
            var response = false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new TestRepository(unitOfWork).UpdateFirstTestStatusToActive();
            }
            return response;
        }

        private uint Insert(string title, string consent)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).Insert(title, consent);
            }
        }

        private uint InsertTestWithTitle(string title)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).InsertTestWithTitle(title);
            }
        }

        private Test GetTest(string title)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestRepository(unitOfWork).GetTest(title);
            }
        }

        private bool CheckDuplicateTitleExist(string title, uint testId)
        {
            var result = GetTest(title);
            return (result == null || (result.Title == title && result.TestId == testId)) ? false : true;
        }

        private bool CheckDuplicateTitleExist(string title)
        {
            var result = GetTest(title);
            return result == null ? false : true;
        }

        #endregion
    }
}