using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class TestCasePropertyService : ITestCasePropertyService
    {
        private static Database _database;

        public TestCasePropertyService()
        {
            _database = new Database(Config.AppSettings);
        }

        public TestCasePropertiesResponse GetTestCaseProperties()
        {
            var result = new TestCasePropertiesResponse();
            List<TestCaseProperty> testCasePropertyList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                testCasePropertyList = new TestCasePropertyRepository(unitOfWork).GetTestCaseProperties();                
            }
            result.TestCaseProperties = testCasePropertyList;
            return result;
        }
    }
}
