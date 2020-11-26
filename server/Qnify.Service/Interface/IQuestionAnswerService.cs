using Qnify.Model;
using Qnify.Model.Table;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface IQuestionAnswerService
    {
        TestSetModel GetQuestionsAnswers();
        List<QuestionModel> GetQuestionsAnswer();
    }
}
