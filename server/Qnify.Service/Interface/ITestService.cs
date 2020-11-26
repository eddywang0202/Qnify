using Qnify.Model;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface ITestService
    {
        IEnumerable<Test> GetTests();
        Test GetTest(uint testId);
        Test GetActiveTest();
        uint GetActiveTestId();        
        Test InsertOrUpdateTest(uint testId, string title, string consent, bool status);
        Test Insert(string title);
        bool Delete(uint testId);
        IEnumerable<uint> GetActiveTestIds();
        string GetConsent(uint testId);
        bool UpdateConsent(uint testId, string consent);
        bool UpdateTitle(uint testId, string title);
        Test UpdateStatus(uint testId, bool status);
    }
}
