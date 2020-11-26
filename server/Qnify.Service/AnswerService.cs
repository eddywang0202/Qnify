using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class AnswerService : IAnswerService
    {
        private static Database _database;

        public AnswerService()
        {
            _database = new Database(Config.AppSettings);
        }

        public IEnumerable<Answer> GetAnswers()
        {
            IEnumerable<Answer> response;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new AnswerRepository(unitOfWork).GetAnswers();
            }
            return response;
        }

        public IEnumerable<Answer> GetAnswer(List<uint> answerIds)
        {
            IEnumerable<Answer> response;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new AnswerRepository(unitOfWork).GetAnswer(answerIds);                
            }
            return response;
        }

        public IEnumerable<Answer> GetAnswer(List<string> answerTitles)
        {
            IEnumerable<Answer> response;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new AnswerRepository(unitOfWork).GetAnswer(answerTitles);
            }
            return response;
        }
    }
}