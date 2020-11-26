using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class UserTestSetAnswerService : IUserTestSetAnswerService
    {
        private static Database _database;

        public UserTestSetAnswerService()
        {
            _database = new Database(Config.AppSettings);
        }

        public UserTestSetAnswersResponse GetUserTestSetAnswers()
        {
            var result = new UserTestSetAnswersResponse();
            List<UserTestSetAnswer> userTestSetAnswerList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                userTestSetAnswerList = new UserTestSetAnswerRepository(unitOfWork).GetUserTestSetAnswers();                
            }
            result.UserTestSetAnswers = userTestSetAnswerList;
            return result;
        }

        public bool Insert(uint testSetId, uint testSetAnswerId, uint userId)
        {
            bool result = false;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                result = new UserTestSetAnswerRepository(unitOfWork).Insert(testSetId, testSetAnswerId, userId);
            }            
            return result;
        }
    }
}
