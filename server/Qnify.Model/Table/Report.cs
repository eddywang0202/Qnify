using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class Report
    {                
        /// <summary>
        /// QuestionTotalCount
        /// </summary>
        [DataMember(Name = "qtc")]
        public uint QuestionTotalCount { get; set; }

        /// <summary>
        /// CorrectAnswerTotalCount
        /// </summary>
        [DataMember(Name = "catc")]
        public uint CorrectAnswerTotalCount { get; set; }

        /// <summary>
        /// ResultPerformance
        /// </summary>
        [DataMember(Name = "rp")]
        public List<ResultPerformance> ResultPerformances { get; set; }

        public Report()
        {
            ResultPerformances = new List<ResultPerformance>();
        }
    }
}
