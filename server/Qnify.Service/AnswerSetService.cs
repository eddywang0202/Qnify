using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class AnswerSetService : IAnswerSetService
    {
        private static Database _database;

        public AnswerSetService()
        {
            _database = new Database(Config.AppSettings);
        }

        public AnswerSetsResponse GetAnswerSets()
        {
            var result = new AnswerSetsResponse();
            List<AnswerSet> answersetList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                answersetList = new AnswerSetRepository(unitOfWork).GetAnswerSets();                
            }
            result.AnswerSets = answersetList;
            return result;
        }
    }
}
