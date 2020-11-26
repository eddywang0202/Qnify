using Qnify.Model;
using Qnify.Model.Response;
using Qnify.Model.Table;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface IDemographicQuestionService
    {
        List<QuestionModel> GetDemographicQuestions();
        UserDemographicModelResponse GetUserDemographicAnswers(uint testId, uint userId);
        (string accessToken, string errorMessage) InsertUserDemographicQuestionAnswer(DemographicQuestionAnswerRequest request);
    }
}
