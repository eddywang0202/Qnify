//using System.Collections.Generic;
//using System.Runtime.Serialization;

//namespace Qnify.Model.Table
//{
//    [DataContract]
//    public class TestQuestionModel
//    {
//        /// <summary>
//        /// QuestionId
//        /// </summary>
//        [DataMember(Name = "qid")]
//        public uint QuestionId { get; set; }

//        /// <summary>
//        /// QuestionTitle
//        /// </summary>
//        [DataMember(Name = "qt")]
//        public string QuestionTitle { get; set; }

//        /// <summary>
//        /// QuestionGroupId
//        /// </summary>
//        [DataMember(Name = "qgid")]
//        public uint QuestionGroupId { get; set; }

//        /// <summary>
//        /// QuestionType
//        /// </summary>
//        [DataMember(Name = "qtid")]
//        public uint QuestionTypeId { get; set; }

//        /// <summary>
//        /// QuestionParentId
//        /// </summary>
//        [DataMember(Name = "qpid")]
//        public uint QuestionParentId { get; set; }

//        /// <summary>
//        /// QuestionOrder
//        /// </summary>
//        [DataMember(Name = "qo")]
//        public uint QuestionOrder { get; set; }

//        /// <summary>
//        /// CorrectAnswerIds
//        /// </summary>
//        [DataMember(Name = "cas")]
//        public List<Answer> CorrectAnswers { get; set; }

//        public TestQuestionModel()
//        {
//            CorrectAnswers = new List<Answer>();
//        }
//    }
//}
