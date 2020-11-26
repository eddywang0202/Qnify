using Qnify.Model;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface ITestSetService
    {
        uint Insert(string title, uint testId);
        bool Update(string title, uint testSetId);
        uint GetTestId(List<uint> testQuestionIds);
    }
}
