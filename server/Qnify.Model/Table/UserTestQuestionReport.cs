using System.Runtime.Serialization;

namespace Qnify.Model.Table
{
    [DataContract]
    public class UserTestQuestionReport
    {
        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "un")]
        public string UserName { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "uid")]
        public uint UserId { get; set; }

        /// <summary>
        /// SubmitDate
        /// </summary>
        [DataMember(Name = "sd")]
        public long? SubmitDate { get; set; }

        /// <summary>
        /// TestName
        /// </summary>
        [DataMember(Name = "tn")]
        public string TestName { get; set; }

        /// <summary>
        /// TestId
        /// </summary>
        [DataMember(Name = "tid")]
        public uint TestId { get; set; }

        /// <summary>
        /// TotalCorrectQuestions
        /// </summary>
        [DataMember(Name = "tcq")]
        public uint TotalCorrectQuestions { get; set; }

        /// <summary>
        /// TotalAnsweredQuestions
        /// </summary>
        [DataMember(Name = "taq")]
        public uint TotalAnsweredQuestions { get; set; }

        /// <summary>
        /// TotalQuestions
        /// </summary>
        [DataMember(Name = "tq")]
        public uint TotalQuestions { get; set; }
    }
}
