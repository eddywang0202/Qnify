using Qnify.DAL;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class TestSetService : ITestSetService
    {
        private static Database _database;

        public TestSetService()
        {
            _database = new Database(Config.AppSettings);
        }

        public uint GetTestId(List<uint> testQuestionIds)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestSetRepository(unitOfWork).GetTestId(testQuestionIds);
            }
        }

        public uint Insert(string title, uint testId)
        {
            var maxOrder = GetMaxOrderId();

            var nextOrder = maxOrder == null ? 1 : maxOrder + 1;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
               return new TestSetRepository(unitOfWork).Insert(title, (uint)nextOrder, testId);                
            }
        }

        public bool Update(string title, uint testSetId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestSetRepository(unitOfWork).Update(title, testSetId);
            }
        }

        public uint? GetMaxOrderId()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new TestSetRepository(unitOfWork).GetMaxOrderId();
            }
        }
    }
}
