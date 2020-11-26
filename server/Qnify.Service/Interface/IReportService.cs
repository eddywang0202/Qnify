using Qnify.Model;
using Qnify.Model.Request;
using Qnify.Model.Response;
using Qnify.Model.Table;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface IReportService
    {
        GetUserTestQuestionReportsResponse GetUserTestQuestionReports(GetUserTestQuestionReportsRequest request);
        ReportSetModelResponse GetUserTestQuestionDetailReport(uint testId, uint userId);
        Report GetUserTestQuestionReport(uint testId, uint userId);
    }
}
