using Microsoft.Extensions.Configuration;
using Qnify.DAL;
using Qnify.Model;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using Qnify.Service.Mapper;
using Qnify.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Qnify.Service
{
    public class QuestionAnswerService : IQuestionAnswerService
    {
        private static Database _database;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ICellService _cellService;
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;

        public QuestionAnswerService(IUserService userService, ITokenService tokenService, IUserProfileService userProfileService,
            IConfiguration configuration, ICellService cellService)
        {
            _database = new Database(Config.AppSettings);
            _userService = userService;
            _tokenService = tokenService;
            _userProfileService = userProfileService;
            _configuration = configuration;
            _cellService = cellService;
        }

        public TestSetModel GetQuestionsAnswers()
        {                        
            var questions = new List<Question>();            

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                questions = new QuestionAnswerRepository(unitOfWork).GetQuestionAnswer();
            }
                        
            return GenerateTestSetModel(questions);
        }

        public List<QuestionModel> GetQuestionsAnswer()
        {
            var result = new List<QuestionModel>();
            var questions = new List<Question>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                questions = new QuestionAnswerRepository(unitOfWork).GetQuestionAnswer();
            }

            if (questions == null || questions.Count == 0) return result;
            return GenerateQuestionAnswerModels(questions);
        }

        public List<QuestionModel> GetQuestionsTemplate()
        {
            var result = new List<QuestionModel>();
            var questions = new List<Question>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                questions = new QuestionAnswerRepository(unitOfWork).GetQuestionAnswer();
            }

            if (questions == null || questions.Count == 0) return result;
            return GenerateQuestionTemplateModels(questions);
        }

        #region Private Method

        private List<QuestionModel> GenerateQuestionTemplateModels(List<Question> questions)
        {
            var questionModels = new List<QuestionModel>();
            var cellInfo = _cellService.GetCell();

            questionModels.AddRange(GenerateQuestionModel(questions.Where(x => x.QuestionGroupId == 2).ToList()));

            foreach (var cellId in cellInfo.Select(x => x.Id).Distinct())
            {
                var questionList = GenerateQuestionModel(questions.Where(x => x.QuestionGroupId == 3).ToList());
                questionList.ForEach(x => x.CellId = cellId);
                questionModels.AddRange(questionList);
            }
            return questionModels;
        }

        private TestSetModel GenerateTestSetModel(List<Question> questions)
        {
            var result = new TestSetModel();

            var questionAnswers = GetQuestionsTemplate();

            result.CaseModels.AddRange(GenerateCaseModel());
            result.TestSetQuestionModels.AddRange(questionAnswers);            

            return result;
        }

        private List<QuestionModel> GenerateQuestionAnswerModels(List<Question> questions)
        {
            var questionModels = new List<QuestionModel>();

            foreach (var questionId in questions.OrderBy(x => x.QuestionGroupId).ThenBy(x => x.QuestionOrder).Select(x => x.QuestionId).Distinct())
            {
                var questionList = questions.Where(x => x.QuestionId == questionId);

                //assign parentId
                if (questionList.FirstOrDefault()?.QuestionId == 18)
                {
                    questionList.Where(x => x.QuestionId == 18).FirstOrDefault().QuestionParentId = 17;
                }

                var questionModel = TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(questionList.FirstOrDefault());

                //re-order answerOrder
                if (questionList.Where(x => x.AnswerOrder == 0).FirstOrDefault() != null)
                {
                    questionList.Where(x => x.AnswerOrder == 0).FirstOrDefault().AnswerOrder = questionList.Max(x => x.AnswerOrder) + 1;
                }

                var answers = questionList.OrderBy(x => x.AnswerOrder).Select(TestQuestionMapper.GenerateAnswer<AnswerServiceModel>);
                questionModel.Answers.AddRange(answers);
                questionModels.Add(questionModel);
            }
            return questionModels;
        }

        private List<QuestionModel> GenerateQuestionModel(List<Question> questions)
        {
            var questionModels = new List<QuestionModel>();

            foreach (var questionId in questions.OrderBy(x => x.QuestionGroupId).ThenBy(x => x.QuestionOrder).Select(x => x.QuestionId).Distinct())
            {
                var questionList = questions.Where(x => x.QuestionId == questionId);

                //assign parentId
                if (questionList.FirstOrDefault()?.QuestionId == 18)
                {
                    questionList.Where(x => x.QuestionId == 18).FirstOrDefault().QuestionParentId = 17;
                }

                var questionModel = TestQuestionMapper.GenerateQuestionModel<QuestionServiceModel>(questionList.FirstOrDefault());

                //re-order answerOrder
                if (questionList.Where(x => x.AnswerOrder == 0).FirstOrDefault() != null)
                {
                    questionList.Where(x => x.AnswerOrder == 0).FirstOrDefault().AnswerOrder = questionList.Max(x => x.AnswerOrder) + 1;
                }

                var answers = questionList.OrderBy(x => x.AnswerOrder).Select(TestQuestionMapper.GenerateAnswer<AnswerServiceModel>);
                questionModel.Answers.AddRange(answers);
                questionModels.Add(questionModel);
            }
            return questionModels;
        }

        private List<CaseServiceModel> GenerateCaseModel()
        {
            var result = new List<CaseServiceModel>();            
            var cellInfo = _cellService.GetCell();

            foreach (var cellId in cellInfo.OrderBy(x => x.Id).Select(x => x.Id).Distinct())
            {
                var testCase = TestQuestionMapper.GenerateCaseModel<CaseServiceModel>(cellInfo.Where(x => x.Id == cellId).FirstOrDefault());

                result.Add(testCase);
            }
            return result;
        }

        #endregion
    }
}
