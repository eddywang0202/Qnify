using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Qnify.Service
{
    public class TestCaseService : ITestCaseService
    {
        private static Database _database;

        public TestCaseService()
        {
            _database = new Database(Config.AppSettings);
        }

        public TestCasesResponse GetTestCases()
        {
            var result = new TestCasesResponse();
            List<TestCase> testcaseList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                testcaseList = new TestCaseRepository(unitOfWork).GetTestCases();
            }

            //foreach (var testSetGroupId in testcaseList.Select(x => x.TestSetGroupId).Distinct())
            //{                
            //    var testCases = testcaseList.Where(x => x.TestSetGroupId == testSetGroupId);
            //    var testCaseCaseList = new List<TestCaseCase>();

            //    foreach (var caseId in testCases.Select(x => x.CaseId).Distinct())
            //    {
            //        var questionModelList = new List<QuestionModel>();

            //        var testCaseList = testCases.Where(x => x.CaseId == caseId).FirstOrDefault();

            //        foreach (var questionId in testCases.Where(x => x.CaseId == caseId).Select(x => x.QuestionId).Distinct())
            //        {
            //            var answerList = new List<Answer>();
                                                
            //            var testCasesAnswers = testCases.Where(x => x.CaseId == caseId && x.QuestionId == questionId);

            //            if (testCasesAnswers.FirstOrDefault().QuestionId != testCasesAnswers.FirstOrDefault().QuestionParentId) continue;

            //            answerList.AddRange(from testCasesAnswer in testCasesAnswers
            //                                select new Answer
            //                                {
            //                                    AnswerValue = testCasesAnswer.AnswerValue,
            //                                    AnswerType = testCasesAnswer.AnswerType,
            //                                    AnswerActionValue = testCasesAnswer.AnswerActionValue,
            //                                    NextQuestionId = testCasesAnswer.NextQuestionId
            //                                });

            //            questionModelList.Add(new QuestionModel
            //            {
            //                QuestionId = questionId,
            //                QuestionValue = testCasesAnswers.FirstOrDefault().QuestionValue,
            //                QuestionParentId = testCasesAnswers.FirstOrDefault().QuestionParentId,
            //                Answers = answerList
            //            });
            //        }

            //        testCaseCaseList.Add(new TestCaseCase
            //        {
            //            CaseId = caseId,
            //            QuestionModels = questionModelList
            //        });
            //    }

            //    var testCase = new TestCaseResponse
            //    {
            //        TestSetGroupId = testCases.FirstOrDefault().TestSetGroupId,
            //        TestCaseCases = testCaseCaseList
            //    };
                
            //    result.TestCaseResponses.Add(testCase);
            //}
            return result;
        }
    }
}
