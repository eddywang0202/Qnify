using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Response;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Service.Mapper;
using Qnify.Utility;
using Qnify.Utility.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qnify.Service
{
    public class ReportService : IReportService
    {
        private static Database _database;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IUserService _userService;
        private readonly ITestService _testService;
        private readonly IQuestionAnswerService _questionAnswerService;


        public ReportService(IQuestionService questionService, IAnswerService answerService, IUserService userService,
            ITestService testService, IQuestionAnswerService questionAnswerService)
        {
            _database = new Database(Config.AppSettings);
            _questionService = questionService;
            _answerService = answerService;
            _userService = userService;
            _testService = testService;
            _questionAnswerService = questionAnswerService;
        }

        public GetUserTestQuestionReportsResponse GetUserTestQuestionReports(GetUserTestQuestionReportsRequest request)
        {
            var response = new GetUserTestQuestionReportsResponse();
            var usersResponse = new UsersResponse();

            var testId = request.TestId ?? 0;

            IEnumerable<User> userList;
            IEnumerable<User> userSearchList = new List<User>();

            if(request.TestId > 0)            
                usersResponse = _userService.GetMemberUsers(testId);
            else
                usersResponse = _userService.GetMemberUsers();

            if (usersResponse == null || usersResponse.Users == null || usersResponse.Users.Count == 0) return response;

            if (string.IsNullOrEmpty(request.Username))
                userList = usersResponse.Users.Skip(request.Offset).Take(request.Limit);
            else
            {
                userSearchList = usersResponse.Users.Where(x => x.Username.Length >= request.Username.Length && x.Username.ToLower().IndexOf(request.Username.ToLower()) >= 0);
                userList = userSearchList.Skip(request.Offset).Take(request.Limit);
            }

            if (userList == null || userList.Count() == 0) return response;

            response = GenerateUsersReportResponse(userList, testId);
            response.TotalPageCount = string.IsNullOrEmpty(request.Username) ? GetTotalPageCount(usersResponse.Users.Count, request.Limit) : GetTotalPageCount(userSearchList.Count(), request.Limit);

            return response;
        }

        public Report GetUserTestQuestionReport(uint testId, uint userId)
        {
            var response = new Report();

            var finalTestId = testId == 0 ?_testService.GetActiveTest().TestId : testId;

            var testSetIds = _questionService.GetTestQuestionList(finalTestId).Select(x => x.TestSetId).ToList();

            var testQuestions = _questionService.GetTestQuestions(testSetIds);
            var userTestQuestionAnswers = _questionService.GetUserTestQuestionAnswer(userId);
            var answers = _answerService.GetAnswers();

            var questionTotalCount = GetTotalQuestionCount(testSetIds);
            var totalCorrectQuestions = CalculateUserTotalCorrectAnswer(testQuestions, userTestQuestionAnswers.Where(x => x.UserId == userId), answers);

            response.QuestionTotalCount = questionTotalCount;
            response.CorrectAnswerTotalCount = totalCorrectQuestions;
            response.ResultPerformances = GenerateResultPerformance(testQuestions, userTestQuestionAnswers, answers);
            return response;
        }

        public ReportSetModelResponse GetUserTestQuestionDetailReport(uint testId, uint userId)
        {
            var response = new ReportSetModelResponse();
            
            var testSetIds = _questionService.GetTestQuestionList(testId).Select(x => x.TestSetId).ToList();
            var testQuestions = _questionService.GetTestQuestions(testSetIds);
            var userTestQuestionAnswers = _questionService.GetUserTestQuestionAnswer(userId);
            var answers = _answerService.GetAnswers();
            var questionAnswers = _questionAnswerService.GetQuestionsAnswer();

            response.ReportSetModels = GenerateReportSetModels(testQuestions, userTestQuestionAnswers, questionAnswers, answers);             
            response.ResultPerformances = GenerateResultPerformance(testQuestions, userTestQuestionAnswers, answers);
            return response;
        }

        #region Private Method

        private List<ReportSetModel> GenerateReportSetModels(List<TestQuestion> testQuestions, IEnumerable<UserQuestionAnswer> userTestQuestionAnswers, List<QuestionModel> questionAnswers,
                                                        IEnumerable<Answer> answers)
        {
            var reportSetModel = new List<ReportSetModel>();            

            foreach (var testSetId in testQuestions.OrderBy(x => x.TestSetOrder).Select(x => x.TestSetId).Distinct())
            {                
                var testSetServiceModel = TestQuestionMapper.GenerateReportSetModel<ReportSetServiceModel>(testQuestions.Where(x => x.TestSetId == testSetId).FirstOrDefault());

                var caseServiceModels = new List<CaseServiceModel>();

                var testSetQuestionModels = questionAnswers.Where(x => x.QuestionGroupId == 2).Select(x => TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(x));

                foreach (var questionAnswer in testSetQuestionModels)
                {                    
                    var testQuestionCorrectAnswer = testQuestions.Where(x => x.TestSetId == testSetId && x.QuestionId == questionAnswer.QuestionId).FirstOrDefault();
                    if (testQuestionCorrectAnswer == null) continue;

                    var userChoosenAnswers = GetUserChoosenAnswers(userTestQuestionAnswers.Where(x => x.TestQuestionId == testQuestionCorrectAnswer.TestQuestionId));
                    var correctAnswers = GetCorrectAnswers(testQuestionCorrectAnswer, answers);
                    var isCorrect = IsCorrectAnswer(userChoosenAnswers, correctAnswers);
                    
                    testSetServiceModel.ReportQuestionModels.Add(GenerateResponse(testQuestionCorrectAnswer, userChoosenAnswers, correctAnswers, isCorrect));
                }

                foreach (var cellId in testQuestions.Where(x => x.TestSetId == testSetId && x.CellId > 0).OrderBy(x => x.CellId).Select(x => x.CellId).Distinct())
                {
                    var testCase = TestQuestionMapper.GenerateCaseModel<CaseServiceModel>(testQuestions.Where(x => x.TestSetId == testSetId && x.CellId == cellId).FirstOrDefault());

                    var questionModels = questionAnswers.Where(x => x.QuestionGroupId == 3).Select(x => TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(x));

                    foreach (var questionAnswer in questionModels)
                    {
                        var testQuestionCorrectAnswer = testQuestions.Where(x => x.TestSetId == testSetId && x.CellId == cellId && x.QuestionId == questionAnswer.QuestionId).FirstOrDefault();
                        var userChoosenAnswers = GetUserChoosenAnswers(userTestQuestionAnswers.Where(x => x.TestQuestionId == testQuestionCorrectAnswer.TestQuestionId));
                        var correctAnswers = GetCorrectAnswers(testQuestionCorrectAnswer, answers);
                        var isCorrect = IsCorrectAnswer(userChoosenAnswers, correctAnswers);

                        testSetServiceModel.ReportQuestionModels.Add(GenerateResponse(testQuestionCorrectAnswer, userChoosenAnswers, correctAnswers, isCorrect, cellId));                        
                    }
                    caseServiceModels.Add(testCase);
                }

                testSetServiceModel.CaseModels.AddRange(caseServiceModels);                
                reportSetModel.Add(testSetServiceModel);
            }
            return reportSetModel;
        }

        private List<ResultPerformance> GenerateResultPerformance(List<TestQuestion> testQuestions, IEnumerable<UserQuestionAnswer> userTestQuestionAnswers, IEnumerable<Answer> answers)
        {
            var response = new List<ResultPerformance>();

            const string questionAbUnormal = "Normal/Abnormal?";
            const string answerAbnormal = "Abnormal";
            const string answerNormal = "Normal";

            var answerAbnormalId = answers.Where(x => x.AnswerTitle == answerAbnormal).SingleOrDefault().AnswerId;
            var answerNormalId = answers.Where(x => x.AnswerTitle == answerNormal).SingleOrDefault().AnswerId;

            var countTN = 0;
            var countFN = 0;
            var countTP = 0;
            var countFP = 0;

            //(correctAnswer)normal + (userAnswer)normal = TN
            countTN = testQuestions.Where(x => x.QuestionTitle == questionAbUnormal &&
            x.CorrectAnswerIds.Split(',').ToList().Select(y => uint.Parse(y)).Contains(answerNormalId) &&
            userTestQuestionAnswers.Where(z => z.TestQuestionId == x.TestQuestionId).SingleOrDefault()?.ChoosenAnswer == answerNormal).Count();

            //(correctAnswer)normal + (userAnswer)normal = FN
            countFN = testQuestions.Where(x => x.QuestionTitle == questionAbUnormal &&
            x.CorrectAnswerIds.Split(',').ToList().Select(y => uint.Parse(y)).Contains(answerNormalId) &&
            userTestQuestionAnswers.Where(z => z.TestQuestionId == x.TestQuestionId).SingleOrDefault()?.ChoosenAnswer == answerAbnormal).Count();

            //(correctAnswer)abnormal + (userAnswer)abnormal = TP
            countTP = testQuestions.Where(x => x.QuestionTitle == questionAbUnormal &&
            x.CorrectAnswerIds.Split(',').ToList().Select(y => uint.Parse(y)).Contains(answerAbnormalId) &&
            userTestQuestionAnswers.Where(z => z.TestQuestionId == x.TestQuestionId).SingleOrDefault()?.ChoosenAnswer == answerAbnormal).Count();

            //(correctAnswer)abnormal + (userAnswer)abnormal = FP
            countFP = testQuestions.Where(x => x.QuestionTitle == questionAbUnormal &&
            x.CorrectAnswerIds.Split(',').ToList().Select(y => uint.Parse(y)).Contains(answerAbnormalId) &&
            userTestQuestionAnswers.Where(z => z.TestQuestionId == x.TestQuestionId).SingleOrDefault()?.ChoosenAnswer == answerNormal).Count();

            var FNTP = (double)countFN + (double)countTP;
            var FPTN = (double)countFP + (double)countTN;
            var TNFP = (double)countTN + (double)countFP;
            var TPFN = (double)countTP + (double)countFN;

            var resultPerformanceDescs = GetConstResultPerformance();

            var falseNegativeRate = FNTP == 0 ? (double)countFN : ((double)countFN / FNTP);
            var falsePositiveRate = FPTN == 0 ? (double)countFP : ((double)countFP / FPTN);
            var trueNegativeRate = TNFP == 0 ? (double)countTN : ((double)countTN / TNFP);
            var truePositiveRate = TPFN == 0 ? (double)countTP : ((double)countTP / TPFN);
            var accuracy = ((double)countTP + (double)countTN) == 0 ?
                0 : ((double)countTP + (double)countTN + (double)countFP + (double)countFN) == 0 ?
                0 : ((double)countTP + (double)countTN) / ((double)countTP + (double)countTN + (double)countFP + (double)countFN);

            var maxRate = Math.Max(truePositiveRate, Math.Max(trueNegativeRate, Math.Max(falseNegativeRate, falsePositiveRate)));

            foreach (var resultPerformanceDesc in resultPerformanceDescs)
            {
                if (resultPerformanceDesc.Code == "fn")
                    response.Add(new ResultPerformance
                    {
                        ResultPerformanceName = resultPerformanceDesc.Description,
                        ResultPerformanceRate = Math.Ceiling(falseNegativeRate * 100) / 100,
                        IsSelected = falseNegativeRate == maxRate ? true : false
                    });

                if (resultPerformanceDesc.Code == "fp")
                    response.Add(new ResultPerformance
                    {
                        ResultPerformanceName = resultPerformanceDesc.Description,
                        ResultPerformanceRate = Math.Ceiling(falsePositiveRate * 100) / 100,
                        IsSelected = falsePositiveRate == maxRate ? true : false
                    });

                if (resultPerformanceDesc.Code == "tn")
                    response.Add(new ResultPerformance
                    {
                        ResultPerformanceName = resultPerformanceDesc.Description,
                        ResultPerformanceRate = Math.Ceiling(trueNegativeRate * 100) / 100,
                        IsSelected = trueNegativeRate == maxRate ? true : false
                    });

                if (resultPerformanceDesc.Code == "tp")
                    response.Add(new ResultPerformance
                    {
                        ResultPerformanceName = resultPerformanceDesc.Description,
                        ResultPerformanceRate = Math.Ceiling(truePositiveRate * 100) / 100,
                        IsSelected = truePositiveRate == maxRate ? true : false
                    });

                if (resultPerformanceDesc.Code == "ac")
                    response.Add(new ResultPerformance
                    {
                        ResultPerformanceName = resultPerformanceDesc.Description,
                        ResultPerformanceRate = Math.Ceiling(accuracy * 100) / 100,
                        IsSelected = false
                    });
            }
            return response;
        }
        
        private ReportQuestionModel GenerateResponse(TestQuestion testQuestion, IEnumerable<string> userAnswers, IEnumerable<string> correctAnswers, bool isCorrect, uint cellId = 0)
        {
            return new ReportQuestionModel
            {
                TestQuestionId = testQuestion.TestQuestionId,
                QuestionId = testQuestion.QuestionId,
                QuestionTitle = testQuestion.QuestionTitle,
                QuestionTypeId = testQuestion.QuestionTypeId,
                QuestionGroupId = testQuestion.QuestionGroupId,
                QuestionParentId = testQuestion.QuestionParentId,
                QuestionOrder = testQuestion.QuestionOrder,
                Answers = userAnswers,
                CorrectAnswers = correctAnswers,
                IsCorrect = isCorrect,
                CellId = cellId
            };
        }

        private GetUserTestQuestionReportsResponse GenerateUsersReportResponse(IEnumerable<User> users, uint testId)
        {
            var response = new GetUserTestQuestionReportsResponse();
            var answers = _answerService.GetAnswers();

            if (testId > 0)
            {
                var testSetIds = _questionService.GetTestQuestionList(testId).Select(x => x.TestSetId).ToList();

                var testQuestions = _questionService.GetTestQuestions(testSetIds);
                var userTestQuestionAnswers = _questionService.GetUsersTestQuestionAnswer(users.Select(x => x.Id).ToList());
                var userTestQuestionAnswerList = userTestQuestionAnswers.Where(x => x.TestId == testId);

                var test = _testService.GetTest(testId);
                var questionTotalCount = GetTotalQuestionCount(testSetIds);

                foreach (var userId in users.OrderBy(x => x.RowNumber).Select(x => x.Id))
                {
                    var user = users.Where(x => x.Id == userId).FirstOrDefault();
                    var totalAnsweredQuestions = GetUserTotalAnsweredCount(userTestQuestionAnswerList, testQuestions, userId);
                    var totalCorrectQuestions = CalculateUserTotalCorrectAnswer(testQuestions, userTestQuestionAnswerList.Where(x => x.UserId == userId), answers);
                    var submitDate = GetSubmitDateTime(userTestQuestionAnswerList, testQuestions, userId);

                    response.UserTestQuestionReports.Add(new UserTestQuestionReport
                    {
                        UserName = user.Username,
                        UserId = user.Id,
                        TestId = test.TestId,
                        TestName = test.Title,
                        TotalQuestions = questionTotalCount,
                        TotalAnsweredQuestions = totalAnsweredQuestions,
                        TotalCorrectQuestions = totalCorrectQuestions,
                        SubmitDate = submitDate
                    });
                }
            }
            else
            {
                var testIds = users.Select(x => x.TestId).Distinct().ToList();
                var testSets = _questionService.GetTestQuestionList(testIds);
                var testSetIds = testSets.Select(x => x.TestSetId).ToList();
                
                var testQuestions = _questionService.GetTestQuestions(testSetIds);
                var userTestQuestionAnswers = _questionService.GetUsersTestQuestionAnswer(users.Select(x => x.Id).ToList());

                var tests = _testService.GetTests();                             

                foreach (var userInfo in users.OrderBy(x => x.RowNumber))
                {                    
                    var questionTotalCount = GetTotalQuestionCount(testSets.Where(x => x.TestId == userInfo.TestId).Select(x => x.TestSetId).ToList());
                    var testQuestionList = testQuestions.Where(x => x.TestId == userInfo.TestId);
                    var userTestQuestionAnswerList = userTestQuestionAnswers.Where(x => x.TestId == userInfo.TestId && x.UserId == userInfo.Id);
                    
                    var totalAnsweredQuestions = GetUserTotalAnsweredCount(userTestQuestionAnswerList, testQuestionList, userInfo.Id);
                    var totalCorrectQuestions = CalculateUserTotalCorrectAnswer(testQuestionList, userTestQuestionAnswerList, answers);
                    var submitDate = GetSubmitDateTime(userTestQuestionAnswerList, testQuestionList, userInfo.Id);

                    var test = tests.Where(x => x.TestId == userInfo.TestId).SingleOrDefault();

                    response.UserTestQuestionReports.Add(new UserTestQuestionReport
                    {
                        UserName = userInfo.Username,
                        UserId = userInfo.Id,
                        TestId = test.TestId,
                        TestName = test.Title,
                        TotalQuestions = questionTotalCount,
                        TotalAnsweredQuestions = totalAnsweredQuestions,
                        TotalCorrectQuestions = totalCorrectQuestions,
                        SubmitDate = submitDate
                    });

                }
            }        
            return response;
        }

        private uint CalculateUserTotalCorrectAnswer(IEnumerable<TestQuestion> testQuestions, IEnumerable<UserQuestionAnswer> userTestQuestionAnswers,
            IEnumerable<Answer> answers)
        {
            uint userTestQuestionAnswerCount = 0;

            foreach (var testQuestionId in testQuestions.Where(x => x.QuestionTitle == "Normal/Abnormal?").Select(x => x.TestQuestionId))
            {
                var testQuestion = testQuestions.Where(x => x.TestQuestionId == testQuestionId).FirstOrDefault();

                if (string.IsNullOrEmpty(testQuestion.CorrectAnswerIds) && testQuestion.QuestionParentId > 0)
                {
                    //check parent question correct answer
                    var isParentQuestionCorrectAnswer = IsCorrectAnswer(
                        testQuestions.Where(x => x.TestQuestionId == testQuestion.QuestionParentId),
                        userTestQuestionAnswers.Where(x => x.TestQuestionId == testQuestion.QuestionParentId),
                        answers);
                    if (isParentQuestionCorrectAnswer) userTestQuestionAnswerCount++;
                    continue;
                }

                var isCorrectAnswer = IsCorrectAnswer(testQuestions.Where(x => x.TestQuestionId == testQuestionId),
                    userTestQuestionAnswers.Where(x => x.TestQuestionId == testQuestionId), answers);
                if (isCorrectAnswer) userTestQuestionAnswerCount++;
            }
            return userTestQuestionAnswerCount;
        }

        private bool IsCorrectAnswer(IEnumerable<TestQuestion> testQuestions, IEnumerable<UserQuestionAnswer> userTestQuestionAnswers, IEnumerable<Answer> answers)
        {
            if (string.IsNullOrEmpty(testQuestions.Select(x => x.CorrectAnswerIds).FirstOrDefault())) return false;

            var correctAnswers = GetCorrectAnswers(testQuestions, answers);
            var userTestQuestionAnswerList = userTestQuestionAnswers.Select(x => x.ChoosenAnswer);

            return correctAnswers.All(x => userTestQuestionAnswerList.Any(y => x.Contains(y))) ? true : false;
        }

        private bool IsCorrectAnswer(IEnumerable<string> userAnswers, IEnumerable<string> correctAnswers)
        {
            if (correctAnswers == null || correctAnswers.Count() == 0) return false;
            return correctAnswers.All(x => userAnswers.Any(y => x.Contains(y))) ? true : false;
        }

        private IEnumerable<string> GetCorrectAnswers(IEnumerable<TestQuestion> testQuestions, IEnumerable<Answer> answers)
        {
            var testQuestionAnswer = testQuestions.Select(x => x.CorrectAnswerIds).FirstOrDefault();
            var testQuestionAnsweIds = testQuestionAnswer.Split(',').ToList().Select(x => uint.Parse(x));
            var answerTitles = answers.Where(x => testQuestionAnsweIds.Contains(x.AnswerId)).Select(x => x.AnswerTitle);
            return answerTitles ?? new List<string>();
        }

        private IEnumerable<string> GetCorrectAnswers(TestQuestion testQuestion, IEnumerable<Answer> answers)
        {
            if (testQuestion == null || string.IsNullOrEmpty(testQuestion.CorrectAnswerIds)) return new List<string>();
            var testQuestionAnswer = testQuestion.CorrectAnswerIds;
            var testQuestionAnsweIds = testQuestionAnswer.Split(',').ToList().Select(x => uint.Parse(x));
            var answerTitles = answers.Where(x => testQuestionAnsweIds.Contains(x.AnswerId)).Select(x => x.AnswerTitle);
            return answerTitles ?? new List<string>();
        }

        private uint GetTotalQuestionCount(List<uint> testSetIds)
        {
            var testQuestions = _questionService.GetTestQuestions(testSetIds);

            return (uint)testQuestions.Where(x => x.QuestionTitle == "Normal/Abnormal?").Count();            
        }

        private uint GetUserTotalAnsweredCount(IEnumerable<UserQuestionAnswer> usersTestQuestionAnswers, IEnumerable<TestQuestion> testQuestions, uint userId)
        {
            var testQuestionIds = testQuestions.Where(x => x.QuestionTypeId != 5).Select(x => x.TestQuestionId);            
            return (uint)usersTestQuestionAnswers.Where(x => x.UserId == userId && testQuestionIds.Contains(x.TestQuestionId)).Select(x => x.TestQuestionId).Distinct().Count();            
        }

        private long? GetSubmitDateTime(IEnumerable<UserQuestionAnswer> usersTestQuestionAnswers, IEnumerable<TestQuestion> testQuestions, uint userId)
        {
            var testQuestionIds = testQuestions.Where(x => x.QuestionTypeId != 5).Select(x => x.TestQuestionId);
            return usersTestQuestionAnswers.Where(x => x.UserId == userId && testQuestionIds.Contains(x.TestQuestionId)).OrderByDescending(x => x.CreatedDateTime).FirstOrDefault()?.CreatedDateTime.ToUnixTimestamp();
        }

        private IEnumerable<string> GetUserChoosenAnswers(IEnumerable<UserQuestionAnswer> usersTestQuestionAnswers)
        {            
            if (usersTestQuestionAnswers == null || usersTestQuestionAnswers.Count() == 0) return new List<string>();
            return usersTestQuestionAnswers.Select(x => x.ChoosenAnswer);
        }

        private long GetTotalPageCount(int totalCount, int limit)
        {
            return (long)Math.Ceiling(((decimal)totalCount / (decimal)limit));
        }

        private List<ConstResultPerformance> GetConstResultPerformance()
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new ConstResultPerformanceRepository(unitOfWork).GetConstResultPerformances();
            }
        }

        #endregion
    }
}