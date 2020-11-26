using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Table;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface IQuestionService
    {

        TestSetModel Insert(TestQuestionRequest request);
        bool Delete(uint testSetId);        
        bool InsertOrUpodateUserTestQuestionAnswer(ParticipantTestQuestionInsertRequest request, uint userId);
        TestSetModel Update(TestQuestionUpdateRequest request);

        TestSetModel GetParticipantTestQuestion(uint testSetId, uint userId = 0);
        TestSetModel GetTestQuestionResponse(uint testSetId);
        List<TestQuestion> GetTestQuestions(List<uint> testSetIds);
        List<TestQuestion> GetTestQuestions();
        List<UserQuestionAnswer> GetUserTestQuestionAnswer(uint userId);
        List<UserQuestionAnswer> GetUserTestQuestionAnswer();
        List<UserQuestionAnswer> GetUsersTestQuestionAnswer(List<uint> userIds);
        List<TestSetListResponse> GetTestQuestionList(uint testId, uint userId = 0);
        List<TestSetListResponse> GetTestQuestionList(List<uint> testIds);
        List<TestSetListResponse> GetActiveTestQuestionList(uint userId = 0);
    }
}
