using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Utility.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qnify.Service.Mapper
{
    public static class TestQuestionMapper
    {
        public static T GenerateQuestionModel<T>(Question question) where T : QuestionModel, new()
        {
            return (new T
            {
                TestQuestionId = question.TestQuestionId,
                QuestionId = question.QuestionId,
                QuestionTitle = question.QuestionTitle,
                QuestionTypeId = question.QuestionTypeId,
                QuestionGroupId = question.QuestionGroupId,
                QuestionParentId = question.QuestionParentId,
                QuestionOrder = question.QuestionOrder
            });
        }

        public static T GenerateQuestionModel<T>(QuestionModel questionModel) where T : QuestionModel, new()
        {
            return (new T
            {
                TestQuestionId = questionModel.TestQuestionId,
                QuestionId = questionModel.QuestionId,
                QuestionTitle = questionModel.QuestionTitle,
                QuestionTypeId = questionModel.QuestionTypeId,
                QuestionGroupId = questionModel.QuestionGroupId,
                QuestionParentId = questionModel.QuestionParentId,
                QuestionOrder = questionModel.QuestionOrder,
                //Answers = questionModel.Answers
            });
        }

        public static T GenerateAnswer<T>(Question question) where T : Answer, new()
        {            
            return (new T
            {
                AnswerId = question.AnswerId,
                AnswerTitle = question.AnswerTitle,
                AnswerOrder = question.AnswerOrder
            });
        }

        public static T GenerateAnswer<T>(Answer answer) where T : Answer, new()
        {
            return (new T
            {
                AnswerId = answer.AnswerId,
                AnswerTitle = answer.AnswerTitle,
                AnswerOrder = answer.AnswerOrder,
                IsChosenAnswer = answer.IsChosenAnswer
            });
        }

        public static T GenerateCaseModel<T>(TestQuestion testQuestion) where T : CaseModel, new()
        {
            return (new T
            {
                CellId = testQuestion.CellId,
                CellRow = testQuestion.CellRow,
                CellColumn = testQuestion.CellColumn,
                CellPropertyJson = testQuestion.CellPropertyJson,
                AdditionalCellPropertyJson = testQuestion.AdditionalCellPropertyJson,
                CellImage = testQuestion.ImagePath
            });
        }

        public static T GenerateCaseModel<T>(Cell cell) where T : CaseModel, new()
        {
            return (new T
            {
                CellId = cell.Id,
                CellRow = cell.Row,
                CellColumn = cell.Column,
                CellPropertyJson = cell.CellPropertyJson
            });
        }

        public static T GenerateTestSetModel<T>(TestQuestion testQuestion) where T : TestSetModel, new()
        {
            return (new T
            {
                TestSetId = testQuestion.TestSetId,
                TestSetTitle = testQuestion.TestSetTitle
            });
        }

        public static T GenerateReportSetModel<T>(TestQuestion testQuestion) where T : ReportSetModel, new()
        {
            return (new T
            {
                TestSetId = testQuestion.TestSetId,
                TestSetTitle = testQuestion.TestSetTitle
            });
        }

        public static T GenerateEasyTokenModel<T>(EasyToken easyToken) where T : EasyTokenModel, new()
        {
            return (new T
            {
                UserId = easyToken.UserId,
                Username = easyToken.Username,
                EasyTokenValue = easyToken.EasyTokenValue,
                Expires = easyToken.Expires.ToUnixTimestamp()
            });
        }
    }
}
