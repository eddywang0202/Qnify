using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class QuestionRepository : BaseRepository
    {
        public QuestionRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        #region Test set

        public bool Delete(uint testSetId)
        {
            const string commandText =
@"DELETE FROM test_set where id = @TestSetId;";

            var param = new DynamicParameters();
            param.Add("@TestSetId", testSetId);            
            return Conn.Execute(commandText, param) > 0;
        }

        #endregion

        #region Test Question

        public bool DeleteTestQuestion(uint testSetId)
        {
            const string commandText =
@"DELETE FROM test_question where test_set_id = @TestSetId;";

            var param = new DynamicParameters();
            param.Add("@TestSetId", testSetId);
            return Conn.Execute(commandText, param) > 0;
        }

        public List<uint> GetTestQuestionCaseIds(uint testSetId)
        {
            const string commandText =
@"SELECT case_id FROM test_question where test_set_id = @TestSetId;";

            var param = new DynamicParameters();
            param.Add("@TestSetId", testSetId);
            var result = Conn.Query<uint>(commandText, param).ToList();
            return result;
        }

        public List<Question> GetQuestion(uint questionGroupId, uint testId = 1)
        {
            const string commandText =
@"SELECT
tq.id as TestQuestionId,
q.id as QuestionId,
q.title as QuestionTitle,
q.order as QuestionOrder,
q.question_group_id as QuestionGroupId,
qt.id as QuestionTypeId,
tq.parent_id as QuestionParentId,
qa.id as QuestionAnswerId,
a.id as AnswerId,
a.title as AnswerTitle,
a.order as AnswerOrder,
qa.next_question_id as AnswerNextQuestionId
FROM test_question tq
INNER JOIN question_answer qa ON qa.question_id = tq.question_id
INNER JOIN question q ON q.id = qa.question_id
INNER JOIN test t ON t.id = q.test_id
INNER JOIN answer a ON a.id = qa.answer_id
INNER JOIN question_group qg ON qg.id = q.question_group_id
INNER JOIN question_type qt ON qt.id = q.question_type_id
WHERE q.question_group_id = @QuestionGroupId AND t.id = @TestId;";

            var param = new DynamicParameters();
            param.Add("@QuestionGroupId", questionGroupId);
            param.Add("@TestId", testId);
            var result = Conn.Query<Question>(commandText, param).ToList();
            return result;
        }

        public List<TestQuestion> GetTestQuestions()
        {
            const string commandText =
@"SELECT
ts.id as TestSetId,
ts.title as TestSetTitle,
ts.order as TestSetOrder,
cell.id as CellId,
cell.row as CellRow,
cell.column as CellColumn,
cell.cell_property_json as CellPropertyJson,
`case`.additional_cell_property_json as AdditionalCellPropertyJson,
`case`.image_path as ImagePath,
tq.id as TestQuestionId,
q.id as QuestionId,
q.title as QuestionTitle,
q.question_group_id as QuestionGroupId,
q.question_type_id as QuestionTypeId,
tq.parent_id as QuestionParentId,
q.order as QuestionOrder,
tq.correct_answer_id as CorrectAnswerIds
FROM test_question tq
LEFT JOIN `case` on `case`.id = tq.case_id
INNER JOIN test_set ts on ts.id = tq.test_set_id
INNER JOIN question q on q.id = tq.question_id
LEFT JOIN cell on cell.id = `case`.cell_id;";

            var result = Conn.Query<TestQuestion>(commandText).ToList();
            return result;
        }

        public List<TestQuestion> GetTestQuestion(uint testSetId)
        {
            const string commandText =
@"SELECT
ts.test_id as TestId,
ts.id as TestSetId,
ts.title as TestSetTitle,
ts.order as TestSetOrder,
cell.id as CellId,
cell.row as CellRow,
cell.column as CellColumn,
cell.cell_property_json as CellPropertyJson,
`case`.additional_cell_property_json as AdditionalCellPropertyJson,
`case`.image_path as ImagePath,
tq.id as TestQuestionId,
q.id as QuestionId,
q.title as QuestionTitle,
q.question_group_id as QuestionGroupId,
q.question_type_id as QuestionTypeId,
tq.parent_id as QuestionParentId,
q.order as QuestionOrder,
tq.correct_answer_id as CorrectAnswerIds
FROM test_question tq
LEFT JOIN `case` on `case`.id = tq.case_id
INNER JOIN test_set ts on ts.id = tq.test_set_id
INNER JOIN question q on q.id = tq.question_id
LEFT JOIN cell on cell.id = `case`.cell_id
WHERE ts.id = @TestSetId;";

            var param = new DynamicParameters();
            param.Add("@TestSetId", testSetId);
            var result = Conn.Query<TestQuestion>(commandText, param).ToList();
            return result;
        }


        public List<TestQuestion> GetTestQuestion(List<uint> testSetIds)
        {
            const string commandText =
@"SELECT
ts.test_id as TestId,
ts.id as TestSetId,
ts.title as TestSetTitle,
ts.order as TestSetOrder,
cell.id as CellId,
cell.row as CellRow,
cell.column as CellColumn,
cell.cell_property_json as CellPropertyJson,
`case`.additional_cell_property_json as AdditionalCellPropertyJson,
`case`.image_path as ImagePath,
tq.id as TestQuestionId,
q.id as QuestionId,
q.title as QuestionTitle,
q.question_group_id as QuestionGroupId,
q.question_type_id as QuestionTypeId,
tq.parent_id as QuestionParentId,
q.order as QuestionOrder,
tq.correct_answer_id as CorrectAnswerIds
FROM test_question tq
LEFT JOIN `case` on `case`.id = tq.case_id
INNER JOIN test_set ts on ts.id = tq.test_set_id
INNER JOIN question q on q.id = tq.question_id
LEFT JOIN cell on cell.id = `case`.cell_id
WHERE ts.id in @TestSetIds;";

            var param = new DynamicParameters();
            param.Add("@TestSetIds", testSetIds);
            var result = Conn.Query<TestQuestion>(commandText, param).ToList();
            return result;
        }

        public List<TestQuestion> GetTestQuestionList()
        {
            const string commandText =
@"SELECT 
id as TestSetId,
title as TestSetTitle,
`order` as TestSetOrder
FROM test_set;";
            var result = Conn.Query<TestQuestion>(commandText).ToList();
            return result;
        }

        public List<TestQuestion> GetTestQuestionList(uint testId)
        {
            const string commandText =
@"SELECT 
id as TestSetId,
title as TestSetTitle,
`order` as TestSetOrder
FROM test_set
WHERE test_id = @TestId;";

            var param = new DynamicParameters();
            param.Add("@TestId", testId);
            var result = Conn.Query<TestQuestion>(commandText, param).ToList();
            return result;
        }

        public List<TestQuestion> GetTestQuestionList(IEnumerable<uint> testIds)
        {
            const string commandText =
@"SELECT 
id as TestSetId,
title as TestSetTitle,
`order` as TestSetOrder,
test_id as TestId
FROM test_set
WHERE test_id IN @TestIds;";

            var param = new DynamicParameters();
            param.Add("@TestIds", testIds);
            var result = Conn.Query<TestQuestion>(commandText, param).ToList();
            return result;
        }

        public bool Insert(uint testSetId, uint caseId, uint questionId, string correctAnswerIds)
        {
            string commandText = questionId == 18 ?

@"INSERT INTO `test_question`
(`test_set_id`,`case_id`,`question_id`,`correct_answer_id`,`parent_id`)
VALUES
(@TestSetId, @CaseId, @QuestionId, @CorrectAnswerId, LAST_INSERT_ID());"
:
@"INSERT INTO `test_question`
(`test_set_id`,`case_id`,`question_id`,`correct_answer_id`)
VALUES
(@TestSetId, @CaseId, @QuestionId, @CorrectAnswerId);";

            var param = new DynamicParameters();
            param.Add("@TestSetId", testSetId);
            param.Add("@CaseId", caseId);
            param.Add("@QuestionId", questionId);
            param.Add("@CorrectAnswerId", correctAnswerIds);

            return Conn.Execute(commandText, param) > 0;
        }

        public bool Update(uint testQuestionId, uint questionId, string correctAnswerIds)
        {
            string commandText = questionId == 18 ?

@"UPDATE test_question
SET
question_id = @QuestionId,
correct_answer_id = @CorrectAnswerId,
parent_id = @ParentId
WHERE id = @TestQuestionId;"
:
@"UPDATE test_question
SET
question_id = @QuestionId,
correct_answer_id = @CorrectAnswerId,
parent_id = null
WHERE id = @TestQuestionId;";

            var param = new DynamicParameters();
            param.Add("@QuestionId", questionId);
            param.Add("@CorrectAnswerId", correctAnswerIds);
            param.Add("@TestQuestionId", testQuestionId);
            param.Add("@ParentId", testQuestionId - 1);

            return Conn.Execute(commandText, param) > 0;
        }

        #endregion

        #region Question

        public List<Question> GetQuestions()
        {
            const string commandText =
@"SELECT
id as QuestionId,
title,
question_type_id as QuestionTypeId,
question_group_id as QuestionGroupId,
test_id as TestId,
`order`
FROM question;";

            var result = Conn.Query<Question>(commandText).ToList();
            return result;
        }

        #endregion
    }
}