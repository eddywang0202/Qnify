using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class TestCaseRepository : BaseRepository
    {
        public TestCaseRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<TestCase> GetTestCases()
        {
            const string commandText =
@"select 
tc.test_case_group_id as TestSetGroupId,
qa.question_id as QuestionId,
q.parent_id as QuestionParentId,
tc.case_id as CaseId,
q.value as QuestionValue,
a.value as AnswerValue,
`at`.value as AnswerType,
ac.value as AnswerActionValue,
qa.next_question_id as NextQuestionId
from test_case tc
inner join question_answer qa on qa.question_id = tc.question_id
inner join question q on q.id = qa.question_id
inner join answer a on a.id = qa.answer_id
inner join answer_action ac on ac.id = qa.answer_action_id
inner join answer_type `at` on `at`.id = a.answer_type_id;";

            var result = Conn.Query<TestCase>(commandText).ToList();
            return result;
        }
    }
}
