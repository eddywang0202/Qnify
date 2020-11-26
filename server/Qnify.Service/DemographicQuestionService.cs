using Microsoft.Extensions.Configuration;
using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Response;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Service.Mapper;
using Qnify.Utility;
using Qnify.Utility.Default;
using System.Collections.Generic;
using System.Linq;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Service
{
    public class DemographicQuestionService : IDemographicQuestionService
    {
        private static Database _database;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;
        private readonly IQuestionService _questionService;
        private readonly ITestService _testService;

        public DemographicQuestionService(IUserService userService, ITokenService tokenService, IUserProfileService userProfileService,
            IConfiguration configuration, IQuestionService questionService, ITestService testService)
        {
            _database = new Database(Config.AppSettings);
            _userService = userService;
            _tokenService = tokenService;
            _userProfileService = userProfileService;
            _configuration = configuration;
            _questionService = questionService;
            _testService = testService;
        }

        public List<QuestionModel> GetDemographicQuestions()
        {
            var result = new QuestionResponse();
            var questions = new List<Question>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                questions = new QuestionRepository(unitOfWork).GetQuestion((uint)QuestionGroup.Demographic);
            }

            return GenerateQuestionModels(questions);
        }

        public UserDemographicModelResponse GetUserDemographicAnswers(uint testId, uint userId)
        {
            var result = new QuestionResponse();
            var questions = new List<Question>();

            var userTestQuestionAnswers = _questionService.GetUserTestQuestionAnswer(userId);

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                questions = new QuestionRepository(unitOfWork).GetQuestion((uint)QuestionGroup.Demographic);
            }

            var questionModels = GenerateQuestionModels(questions);
            return GenerateUserDemographicResponse(questionModels, userTestQuestionAnswers.Where(x => x.TestId == testId));
        }

        private UserDemographicModelResponse GenerateUserDemographicResponse(List<QuestionModel> questionModels, IEnumerable<UserQuestionAnswer> userQuestionAnswers)
        {
            var response = new UserDemographicModelResponse();
            var userDemographicModels = new List<UserDemographicModel>();

            userDemographicModels.AddRange(from question in questionModels
                                           let userChoosenAnswers = userQuestionAnswers.Where(x => x.TestQuestionId == question.TestQuestionId).Select(x => x.ChoosenAnswer)
                                           select new UserDemographicModel
                                           {
                                               QuestionTitle = question.QuestionTitle,
                                               Answers = userChoosenAnswers
                                           });
            response.UserName = userDemographicModels.Where(x => x.QuestionTitle == "Name").FirstOrDefault()?.Answers.FirstOrDefault();
            response.UserDemographicModels = userDemographicModels;
            if (string.IsNullOrEmpty(response.UserName)) return new UserDemographicModelResponse();
            return response;
        }

        public (string accessToken, string errorMessage) InsertUserDemographicQuestionAnswer(DemographicQuestionAnswerRequest request)
        {
            var result = false;
            string accessToken = "";

            var testId = _testService.GetActiveTest().TestId;

            //Get demographic test question
            var demoGrahpicTestQuestions = GetDemographicQuestions();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                var username = request.UserQuestionAnswers.Where(x => x.TestQuestionId == demoGrahpicTestQuestions.Where(y => y.QuestionTitle == "Name").FirstOrDefault()?.TestQuestionId).FirstOrDefault().ChoosenAnswer;
                var age = request.UserQuestionAnswers.Where(x => x.TestQuestionId == demoGrahpicTestQuestions.Where(y => y.QuestionTitle == "Age").FirstOrDefault()?.TestQuestionId).FirstOrDefault().ChoosenAnswer;
                var email = request.UserQuestionAnswers.Where(x => x.TestQuestionId == demoGrahpicTestQuestions.Where(y => y.QuestionTitle == "Email address").FirstOrDefault()?.TestQuestionId).FirstOrDefault().ChoosenAnswer;

                var easyToken = _tokenService.GetTokenInfo(request.EasyToken);
                var resultValidateToken = ValidateToken(easyToken);

                if (!resultValidateToken.validation) return (null, resultValidateToken.errorMessage);

                var userId = _userService.InsertUser(username.Trim());
                var resultUpdateTokenUser = _tokenService.UpdateTokenUser(request.EasyToken, userId);
                var resultInsertUserProfile = _userProfileService.InsertUserProfile(username, age, email, userId);

                foreach (var userQuestionAnswer in request.UserQuestionAnswers)
                {
                    result = new UserTestQuestionAnswerRepository(unitOfWork).InsertOrUpodateUserTestQuestionAnswer(
                        userQuestionAnswer.TestQuestionId, userQuestionAnswer.ChoosenAnswer, userId, testId);
                }

                if (result)
                    accessToken = TokenHelper.GenerateAccessToken(username, DefaultValue.Role, _configuration["Token:Secret"], _configuration["Token:Issuer"], userId);
            }
            return (accessToken, null);
        }

        #region Private Method

        private List<QuestionModel> GenerateQuestionModels(List<Question> questions)
        {
            var questionModels = new List<QuestionModel>();

            foreach (var questionId in questions.Select(x => x.QuestionId).Distinct())
            {
                var questionList = questions.Where(x => x.QuestionId == questionId);
                var questionModel = TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(questionList.FirstOrDefault());                

                var answers = GenerateAnswer(questionList);
                questionModel.Answers.AddRange(answers);
                questionModels.Add(questionModel);
            }
            return questionModels;
        }

        private IEnumerable<AnswerServiceModel> GenerateAnswer(IEnumerable<Question> questionList)
        {
            //re-order answerOrder
            if (questionList.Where(x => x.AnswerOrder == 0).FirstOrDefault() != null)
            {
                questionList.Where(x => x.AnswerOrder == 0).FirstOrDefault().AnswerOrder = questionList.Max(x => x.AnswerOrder) + 1;
            }

            var answers = questionList.OrderBy(x => x.AnswerOrder).Select(TestQuestionMapper.GenerateAnswer<AnswerServiceModel>);
            return answers;
        }

        private (bool validation, string errorMessage) ValidateToken(EasyTokenModel easyToken)
        {
            return easyToken != null ? easyToken.Username == null ? (true, "") : (false, "Duplicate username") : (false, "Invalid Token");
        }

        #endregion
    }
}
