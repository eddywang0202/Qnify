using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Service.Mapper;
using Qnify.Utility;
using Qnify.Utility.Default;
using System.Collections.Generic;
using System.Linq;

namespace Qnify.Service
{
    public class QuestionService : IQuestionService
    {
        private static Database _database;
        private readonly ITestSetService _testSetService;
        private readonly ICaseService _caseService;
        private readonly IAnswerService _answerService;
        private readonly IQuestionAnswerService _questionAnswerService;
        private readonly ITestService _testService;

        public QuestionService(ITestSetService testSetService, ICaseService caseService, IAnswerService answerService,
            IQuestionAnswerService questionAnswerService, ITestService testService)
        {
            _database = new Database(Config.AppSettings);
            _testSetService = testSetService;
            _caseService = caseService;
            _answerService = answerService;
            _questionAnswerService = questionAnswerService;
            _testService = testService;
        }

        public TestSetModel Insert(TestQuestionRequest request)
        {            
            var resultInsert = false;
            var questions = GetQuestions();
            var optionalquestionIds = questions.Where(x => x.QuestionTypeId == 5).Select(x => x.QuestionId);

            //Insert test set
            var testSetId = _testSetService.Insert(request.TestSetTitle, request.TestId);

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                //insert test set question answer
                foreach (var testSetQuestionModel in request.TestSetQuestionModels.Where(x => x.CellId == 0))
                {
                    new QuestionRepository(unitOfWork).Insert(testSetId, 0, testSetQuestionModel.QuestionId,
                        testSetQuestionModel.AnswerIds == null ?
                        "" : optionalquestionIds.Contains(testSetQuestionModel.QuestionId) ?
                        "" : string.Join(",", testSetQuestionModel.AnswerIds.ToArray()));
                }

                //insert test case question answer
                foreach (var caseModel in request.CaseModels)
                {
                    var caseId = _caseService.Insert(caseModel.CellId, caseModel.AdditionalCellPropertyJson, caseModel.CellImage);

                    foreach (var testSetQuestionModel in request.TestSetQuestionModels.Where(x => x.CellId == caseModel.CellId))
                    {
                        resultInsert = new QuestionRepository(unitOfWork).Insert(testSetId, caseId, testSetQuestionModel.QuestionId,
                            testSetQuestionModel.AnswerIds == null ?
                            "" : string.Join(",", testSetQuestionModel.AnswerIds.ToArray()));
                    }
                }
            }

            return GetTestQuestionResponse(testSetId);
        }

        public TestSetModel Update(TestQuestionUpdateRequest request)
        {
            var questions = GetQuestions();
            var optionalquestionIds = questions.Where(x => x.QuestionTypeId == 5).Select(x => x.QuestionId);

            //Update Testset title
            if (request.TestSetTitle != null)
            {
                var resultUpdateTestsetTitle = _testSetService.Update(request.TestSetTitle, request.TestSetId);
            }

            //Update cell info
            if (request.CaseModels.Count != 0)
            {
                foreach (var cellId in request.CaseModels.Select(x => x.CellId).Distinct())
                {
                    var cellInfo = request.CaseModels.Where(x => x.CellId == cellId).FirstOrDefault();
                    _caseService.Update(request.TestSetId, cellInfo.CellId, cellInfo.AdditionalCellPropertyJson, cellInfo.CellImage);
                }
            }

            //Update test question
            if (request.TestSetQuestionModels.Count != 0)
            {
                foreach (var testQuestionId in request.TestSetQuestionModels.Select(x => x.TestQuestionId).Distinct())
                {
                    var testQuestionModel = request.TestSetQuestionModels.Where(x => x.TestQuestionId == testQuestionId).FirstOrDefault();
                    UpdateTestQuestion(testQuestionModel.TestQuestionId, testQuestionModel.QuestionId,
                        testQuestionModel.AnswerIds == null ?
                        "" : optionalquestionIds.Contains(testQuestionModel.QuestionId) ?
                        "" : string.Join(",", testQuestionModel.AnswerIds.ToArray()));
                }
            }

            return GetTestQuestionResponse(request.TestSetId);
        }

        public bool Delete(uint testSetId)
        {
            var caseIds = GetTestQuestionCaseIds(testSetId);

            if (caseIds == null || caseIds.Count == 0) return false;

            var caseInfos = _caseService.GetCases(caseIds.Distinct().ToList());

            var resultDeleteTestSet = false;
            var resultDeleteTestQuestion = false;
            var resultDeleteCases = false;

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                resultDeleteTestSet = new QuestionRepository(unitOfWork).Delete(testSetId);
                resultDeleteTestQuestion = new QuestionRepository(unitOfWork).DeleteTestQuestion(testSetId);
            }

            resultDeleteCases = _caseService.Delete(caseInfos.Cases.Select(x => x.Id).ToList());

            return (resultDeleteTestSet && resultDeleteTestSet  && resultDeleteTestQuestion) ? true : false;
        }

        public TestSetModel GetParticipantTestQuestion(uint testSetId, uint userId = 0)
        {
            var result = new TestSetModel();

            var testQuestionsResponse = GetTestQuestion(testSetId);

            if (testQuestionsResponse.Count == 0) return result;

            var testSetModels = GenerateSetModels(testQuestionsResponse, true, userId);
            var testSetModel = testSetModels.FirstOrDefault();
            return testSetModel;
        }

        public bool UpdateTestQuestion(uint testQuestionId, uint questionId, string correctAnswerIds)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
               return new QuestionRepository(unitOfWork).Update(testQuestionId, questionId, correctAnswerIds);
            }
        }

        public TestSetModel GetTestQuestionResponse(uint testSetId)
        { 
            var testQuestionsResponse = new List<TestQuestion>();

            testQuestionsResponse = GetTestQuestion(testSetId);

            var testSetModels = GenerateSetModels(testQuestionsResponse);
            var testSetModel = testSetModels.FirstOrDefault();
            return testSetModel;
        }

        public List<TestQuestion> GetTestQuestion(uint testSetId)
        {            
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new QuestionRepository(unitOfWork).GetTestQuestion(testSetId);
            }            
        }

        public List<TestQuestion> GetTestQuestions()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new QuestionRepository(unitOfWork).GetTestQuestions();
            }
        }

        public List<TestQuestion> GetTestQuestions(List<uint> testSetIds)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new QuestionRepository(unitOfWork).GetTestQuestion(testSetIds);
            }
        }

        public List<UserQuestionAnswer> GetUserTestQuestionAnswer(uint userId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserTestQuestionAnswerRepository(unitOfWork).GetUserTestQuestionAnswer(userId);
            }
        }

        public List<UserQuestionAnswer> GetUserTestQuestionAnswer()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserTestQuestionAnswerRepository(unitOfWork).GetUserTestQuestionAnswer();
            }
        }

        public List<UserQuestionAnswer> GetUsersTestQuestionAnswer(List<uint> userIds)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserTestQuestionAnswerRepository(unitOfWork).GetUsersTestQuestionAnswer(userIds);
            }
        }

        public List<TestSetListResponse> GetTestQuestionList(uint testId, uint userId = 0)
        {
            var response = new List<TestSetListResponse>();
            var testSets = new List<TestQuestion>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                testSets = new QuestionRepository(unitOfWork).GetTestQuestionList(testId);      
            }
            if (testSets == null || testSets.Count == 0) return response;
            return GetTestSetList(userId, testSets);
        }

        public List<TestSetListResponse> GetTestQuestionList(List<uint> testIds)
        {
            var response = new List<TestSetListResponse>();
            var testSets = new List<TestQuestion>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                testSets = new QuestionRepository(unitOfWork).GetTestQuestionList(testIds);
            }
            if (testSets == null || testSets.Count == 0) return response;
            return GetTestSetList(testSets);
        }

        public List<TestSetListResponse> GetActiveTestQuestionList(uint userId = 0)
        {
            var response = new List<TestSetListResponse>();
            var testSets = new List<TestQuestion>();

            var activeTestId = _testService.GetActiveTestId();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                testSets = new QuestionRepository(unitOfWork).GetTestQuestionList(activeTestId);
            }
            if (testSets == null || testSets.Count == 0) return response;
            return GetTestSetList(userId, testSets);
        }

        private List<TestSetListResponse> GetTestSetList(uint userId, List<TestQuestion> testSets)
        {
            var testSetListResponse = new List<TestSetListResponse>();            
            var testQuestions = new List<TestQuestion>();
            var userTestQuestionAnswers = new List<UserQuestionAnswer>();

            if (userId != 0)
            {
                using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
                {
                    userTestQuestionAnswers = GetUserTestQuestionAnswer(userId);
                    testQuestions = new QuestionRepository(unitOfWork).GetTestQuestion(testSets.Select(x => x.TestSetId).ToList());
                }
            }
            
            testSetListResponse.AddRange(from testSet in testSets
                                         let testQuestionIds = testQuestions.Where(x => x.TestSetId == testSet.TestSetId).Select(x => x.TestQuestionId)
                                         let isContainUserAnswerIds = testQuestionIds.Any(x => userTestQuestionAnswers.Select(y => y.TestQuestionId).Contains(x))
                                         select new TestSetListResponse
                                         {
                                             TestSetId = testSet.TestSetId,
                                             TestSetTitle = testSet.TestSetTitle,
                                             TestSetOrder = testSet.TestSetOrder,
                                             IsAnswered = isContainUserAnswerIds
                                         });
            return testSetListResponse;
        }

        private List<TestSetListResponse> GetTestSetList(List<TestQuestion> testSets)
        {
            return testSets.Select(x => new TestSetListResponse
            {
                TestSetId = x.TestSetId,
                TestId = x.TestId
            }).ToList();
        }

        public bool InsertOrUpodateUserTestQuestionAnswer(ParticipantTestQuestionInsertRequest request, uint userId)
        {
            var result = false;            

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {                
                var answers = _answerService.GetAnswer(request.UserQuestionAnswers.SelectMany(x => x.AnswerIds).ToList());
                var testId = _testSetService.GetTestId(request.UserQuestionAnswers.Select(x => x.TestQuestionId).Distinct().ToList());

                foreach (var userQuestionAnswer in request.UserQuestionAnswers)
                {
                    //delete user test question answer before insert or update
                    new UserTestQuestionAnswerRepository(unitOfWork).DeleteUserTestQuestionAnswer(userQuestionAnswer.TestQuestionId, userId);

                    foreach (var answerId in userQuestionAnswer.AnswerIds)
                    {
                        var answerInfo = answers.Where(x => x.AnswerId == answerId).FirstOrDefault();
                        var answer = answerInfo != null ? answerInfo.AnswerTitle : null;

                        result = new UserTestQuestionAnswerRepository(unitOfWork).InsertOrUpodateUserTestQuestionAnswer(userQuestionAnswer.TestQuestionId, answer, userId, testId);
                    }
                }
            }
            return result;
        }

        #region Private Method

        private List<uint> GetTestQuestionCaseIds(uint testSetId)
        {
            var result = new List<uint>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                result = new QuestionRepository(unitOfWork).GetTestQuestionCaseIds(testSetId);
            }

            return result;
        }

        private List<Question> GetQuestions()
        {
            var result = new List<Question>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                result = new QuestionRepository(unitOfWork).GetQuestions();
            }

            return result;
        }

        private List<TestSetModel> GenerateSetModels(List<TestQuestion> testQuestions, bool isParticipantTestQuestions = false, uint userId = 0)
        {
            var testSetModels = new List<TestSetModel>();
            var questionAnswers = _questionAnswerService.GetQuestionsAnswer();

            foreach (var testSetId in testQuestions.OrderBy(x => x.TestSetOrder).Select(x => x.TestSetId).Distinct())
            {
                var userTestQuestionAnswer = GetUserTestQuestionAnswer(userId);

                var testSetServiceModel = TestQuestionMapper.GenerateTestSetModel<TestSetServiceModel>(testQuestions.Where(x => x.TestSetId == testSetId).FirstOrDefault());

                var caseServiceModels = new List<CaseServiceModel>();

                var testSetQuestionModels = questionAnswers.Where(x => x.QuestionGroupId == 2).Select(x => TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(x));

                foreach (var questionAnswer in testSetQuestionModels)
                {
                    var result = TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(questionAnswer);
                    result.Answers.AddRange(questionAnswers.Where(x => x.QuestionId == questionAnswer.QuestionId).SelectMany(x => x.Answers).Select(y => TestQuestionMapper.GenerateAnswer<AnswerServiceModel>(y)));

                    var testQuestionCorrectAnswer = testQuestions.Where(x => x.TestSetId == testSetId && x.QuestionId == result.QuestionId).FirstOrDefault();
                    if (testQuestionCorrectAnswer == null) continue;

                    result.TestQuestionId = testQuestionCorrectAnswer.TestQuestionId;
                    result.QuestionParentId = testQuestionCorrectAnswer.QuestionParentId;
                    
                    var correctAnswers = testQuestionCorrectAnswer.CorrectAnswerIds != "" ? testQuestionCorrectAnswer.CorrectAnswerIds.Split(',').ToList().Select(x => uint.Parse(x)) : null;
                    if (correctAnswers != null || questionAnswer.QuestionTypeId == 5)
                    {
                        if (!isParticipantTestQuestions && questionAnswer.QuestionTypeId != 5)
                            result.Answers.Where(x => correctAnswers.Contains(x.AnswerId)).ToList().ForEach(x => x.IsChosenAnswer = true);
                        else
                            GetUserChoosenAnswer(userTestQuestionAnswer, result);

                    }
                    testSetServiceModel.TestSetQuestionModels.Add(result);
                }

                foreach (var cellId in testQuestions.Where(x => x.TestSetId == testSetId && x.CellId > 0).OrderBy(x => x.CellId).Select(x => x.CellId).Distinct())
                {
                    var testCase = TestQuestionMapper.GenerateCaseModel<CaseServiceModel>(testQuestions.Where(x => x.TestSetId == testSetId && x.CellId == cellId).FirstOrDefault());

                    var questionModels = questionAnswers.Where(x => x.QuestionGroupId == 3).Select(x => TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(x));

                    foreach (var questionAnswer in questionModels)
                    {
                        var result = TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(questionAnswer);
                        result.Answers.AddRange(questionAnswers.Where(x => x.QuestionId == questionAnswer.QuestionId).SelectMany(x => x.Answers).Select(y => TestQuestionMapper.GenerateAnswer<AnswerServiceModel>(y)));

                        var testQuestionCorrectAnswer = testQuestions.Where(x => x.TestSetId == testSetId && x.CellId == cellId && x.QuestionId == result.QuestionId).FirstOrDefault();

                        result.CellId = cellId;
                        result.TestQuestionId = testQuestionCorrectAnswer.TestQuestionId;
                        result.QuestionParentId = testQuestionCorrectAnswer.QuestionParentId;

                        if (!isParticipantTestQuestions)
                        {
                            var correctAnswers = testQuestionCorrectAnswer.CorrectAnswerIds != "" ? testQuestionCorrectAnswer.CorrectAnswerIds.Split(',').ToList().Select(x => uint.Parse(x)) : null;
                            if (correctAnswers != null)
                            {
                                result.Answers.Where(x => correctAnswers.Contains(x.AnswerId)).ToList().ForEach(x => x.IsChosenAnswer = true);
                            }
                        }
                        else
                        {
                            GetUserChoosenAnswer(userTestQuestionAnswer, result);
                        }                     
                        testSetServiceModel.TestSetQuestionModels.Add(result);
                    }
                    caseServiceModels.Add(testCase);
                }

                testSetServiceModel.CaseModels.AddRange(caseServiceModels);
                testSetModels.Add(testSetServiceModel);
            }
            return testSetModels;
        }

        #endregion

        #region Private Method

        private void GetUserChoosenAnswer(List<UserQuestionAnswer> userTestQuestionAnswer, QuestionServiceModel result)
        {
            //Get user test question answer to assign ica
            var answers = _answerService.GetAnswer(userTestQuestionAnswer.Where(x => x.TestQuestionId == result.TestQuestionId).Select(x => x.ChoosenAnswer).ToList());
            var userAnswerIds = answers.Select(x => x.AnswerId);
                      
            result.Answers.Where(x => userAnswerIds.Contains(x.AnswerId)).ToList().ForEach(x => x.IsChosenAnswer = true);
        }

        #endregion
    }
}
