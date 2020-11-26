using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Response
{
    [DataContract]
    public class GetUserTestQuestionReportsResponse
    {
        /// <summary>
        /// TotalPageCount
        /// </summary>
        [DataMember(Name = "tpc")]
        public long TotalPageCount { get; set; }

        /// <summary>
        /// userTestQuestionReports
        /// </summary>
        [DataMember(Name = "utqr")]
        public List<UserTestQuestionReport> UserTestQuestionReports { get; set; }

        public GetUserTestQuestionReportsResponse()
        {
            UserTestQuestionReports = new List<UserTestQuestionReport>();
        }
    }
}
