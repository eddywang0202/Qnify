using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Response
{
    [DataContract]
    public class ReportSetModelResponse
    {
        /// <summary>
        /// ReportSetModels
        /// </summary>
        [DataMember(Name = "rsm")]
        public List<ReportSetModel> ReportSetModels { get; set; }

        /// <summary>
        /// ResultPerformance
        /// </summary>
        [DataMember(Name = "rp")]
        public List<ResultPerformance> ResultPerformances { get; set; }

        public ReportSetModelResponse()
        {
            ReportSetModels = new List<ReportSetModel>();
            ResultPerformances = new List<ResultPerformance>();
        }
    }
}
