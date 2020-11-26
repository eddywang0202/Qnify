using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class ResultPerformanceRateModel
    {
        /// <summary>
        /// ResultPerformanceName
        /// </summary>
        [DataMember(Name = "rpn")]
        public string ResultPerformanceName { get; set; }

        /// <summary>
        /// ResultPerformanceRate
        /// </summary>
        [DataMember(Name = "rpr")]
        public double ResultPerformanceRate { get; set; }

        /// <summary>
        /// IsSelected
        /// </summary>
        [DataMember(Name = "is")]
        public bool IsSelected { get; set; }
    }
}
