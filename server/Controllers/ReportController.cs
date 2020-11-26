using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Response;
using Qnify.Model.Table;
using Qnify.Service.Interface;
using System.Collections.Generic;
using static Qnify.Utility.TokenGenerator;

namespace Qnify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Participant/testresultlist")]
        public ActionResult<GetUserTestQuestionReportsResponse> GetUserTestQuestionReport([FromBody] GetUserTestQuestionReportsRequest request)
        {
            if(request.Limit == 0) return BadRequest("Limit cannot be null");
            return _reportService.GetUserTestQuestionReports(request);            
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Participant/testresultdetail/{testId}/{userId}")]
        public ActionResult<ReportSetModelResponse> GetUserTestQuestionReport(uint userId, uint testId)
        {
            if (userId == 0) return BadRequest("User id cannot be null");
            var result = _reportService.GetUserTestQuestionDetailReport(testId, userId);
            if (result == null) return BadRequest("No user record found");
            return result;
        }

        [Authorize(Roles = "member")]
        [HttpGet("Participant/{testId}")]
        public ActionResult<Report> GetUserTestQuestionReport(uint testId)
        {
            var hearder = HttpContext.Request.Headers["Authorization"].ToString();

            var userId = TokenHelper.GetAccessTokenUserId(hearder);
            if (userId == 0) return BadRequest("No user id found");
            return _reportService.GetUserTestQuestionReport(testId, userId);
        }

    }
}
