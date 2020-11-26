using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;
using Qnify.Utility.Default;

namespace Qnify.DAL
{
    public class QuestionAnswerRepository : BaseRepository
    {
        public QuestionAnswerRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<Question> GetQuestionAnswer()
        {
            const string commandText =
@"SELECT
q.id as QuestionId,
q.title as QuestionTitle,
q.question_group_id as QuestionGroupId,
q.order as QuestionOrder,
qt.id as QuestionTypeId,
a.id as AnswerId,
a.title as AnswerTitle,
a.order as AnswerOrder,
qa.next_question_id as AnswerNextQuestionId
FROM question_answer qa
INNER JOIN question q ON q.id = qa.question_id
INNER JOIN answer a ON a.id = qa.answer_id
INNER JOIN question_type qt ON qt.id = q.question_type_id
WHERE q.question_group_id > @QuestionGroupId
ORDER BY q.question_group_id, q.order;";

            var param = new DynamicParameters();
            param.Add("@QuestionGroupId", (uint)QuestionGroup.Demographic);            
            var result = Conn.Query<Question>(commandText, param).ToList();
            return result;
        }
    }
}
