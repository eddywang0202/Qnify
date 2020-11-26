using Qnify.Model;

namespace Qnify.Service.Interface
{
    public interface IUserTestCaseAnswerService
    {
        UserTestCaseAnswersResponse GetUserTestCaseAnswers();
        bool Insert(uint testCaseId, uint testCaseAnswerId, uint userId);
    }
}
