using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class AnswerSetRepository : BaseRepository
    {
        public AnswerSetRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<AnswerSet> GetAnswerSets()
        {
            const string commandText =
@"SELECT `id`,
`question_id` as QuestionId,
`value`,
`answer_action_id` as AnswerActionId,
`next_question_id` as NextQuestionId
FROM `answer_set`";

            var result = Conn.Query<AnswerSet>(commandText).ToList();
            return result;
        }
    }
}
