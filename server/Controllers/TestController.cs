using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Qnify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly IQuestionService _questionService;

        public TestController(ITestService testService, IQuestionService questionService)
        {
            _testService = testService;
            _questionService = questionService;
        }

        [Authorize(Roles = "admin, guest, member")]
        [HttpGet("List")]
        public ActionResult<List<Test>> GetTests()
        {
            var result = _testService.GetTests();
            return result.ToList();
        }

        [HttpGet]
        public ActionResult<Test> GetActiveTest()
        {
            return GetActiveTestService();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Consent/Update/{testId}")]
        public ActionResult<bool> UpdateConsent(uint testId, [FromBody] string consent)
        {
            return _testService.UpdateConsent(testId, consent);
        }

        [Authorize(Roles = "admin, member, guest")]
        [HttpGet("Consent/{testId}")]
        public ActionResult<string> GetConsent(uint testId)
        {
            return _testService.GetConsent(testId);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Title/Update/{testId}")]
        public ActionResult<bool> UpdateTitle(uint testId, [FromBody] string title)
        {
            return _testService.UpdateTitle(testId, title);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Status/Update/{testId}")]
        public ActionResult<Test> UpdateStatus(uint testId, [FromBody] bool status)
        {
            return _testService.UpdateStatus(testId, status);
        }

        //[Authorize(Roles = "admin")]
        //[HttpPost("Update")]
        //public ActionResult<Test> Update(uint testId, [FromBody] TestRequest request)
        //{
        //    var result = _testService.InsertOrUpdateTest(request.TestId, request.Title, request.Consent, request.Status);
        //    if (result == null) return BadRequest("Test title had been used");
        //    return result;
        //}

        [Authorize(Roles = "admin")]
        [HttpPost("Insert")]
        public ActionResult<Test> Insert([FromBody] string title)
        {
            if (string.IsNullOrEmpty(title)) return BadRequest("Title cannot be null");
            var result = _testService.Insert(title);
            if (result == null) return BadRequest("Test title had been used");
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{testId}")]
        public ActionResult<bool> Delete(uint testId)
        {
            if (testId == 0) return BadRequest("test id cannot be null");
            var result = _testService.Delete(testId);
            if (!result) return BadRequest("No record found");
            return result;
        }

        #region Private Method

        private Test GetActiveTestService()
        {
            var activeTest = _testService.GetActiveTest();
            if (activeTest == null || activeTest.TestId == 0) return new Test();

            var testQuestionList = _questionService.GetTestQuestionList(activeTest.TestId);

            activeTest.Status = (testQuestionList == null || !testQuestionList.Any()) ? false : true;

            return activeTest;
        }

        #endregion
    }
}
