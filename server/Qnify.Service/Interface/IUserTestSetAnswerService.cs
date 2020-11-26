using Qnify.Model;

namespace Qnify.Service.Interface
{
    public interface IUserTestSetAnswerService
    {
        UserTestSetAnswersResponse GetUserTestSetAnswers();
        bool Insert(uint testSetId, uint testSetAnswerId, uint userId);
    }
}
