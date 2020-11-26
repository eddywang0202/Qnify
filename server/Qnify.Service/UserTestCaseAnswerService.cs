using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class UserTestCaseAnswerService : IUserTestCaseAnswerService
    {
        private static Database _database;

        public UserTestCaseAnswerService()
        {
            _database = new Database(Config.AppSettings);
        }

        public UserTestCaseAnswersResponse GetUserTestCaseAnswers()
        {
            var result = new UserTestCaseAnswersResponse();
            List<UserTestCaseAnswer> userTestCaseAnswerList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                userTestCaseAnswerList = new UserTestCaseAnswerRepository(unitOfWork).GetUserTestCaseAnswers();                
            }
            result.UserTestCaseAnswers = userTestCaseAnswerList;
            return result;
        }

        public bool Insert(uint testCaseId, uint testCaseAnswerId, uint userId)
        {
            bool result = false;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                result = new UserTestCaseAnswerRepository(unitOfWork).Insert(testCaseId, testCaseAnswerId, userId);
            }
            return result;
        }
    }
}
