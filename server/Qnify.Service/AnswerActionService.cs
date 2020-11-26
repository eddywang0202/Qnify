using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class AnswerActionService : IAnswerActionService
    {
        private static Database _database;

        public AnswerActionService()
        {
            _database = new Database(Config.AppSettings);
        }

        public AnswerActionsResponse GetAnswerActions()
        {
            var result = new AnswerActionsResponse();
            List<AnswerAction> answerActionList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                answerActionList = new AnswerActionRepository(unitOfWork).GetAnswerActions();                
            }
            result.AnswerActions = answerActionList;
            return result;
        }
    }
}
